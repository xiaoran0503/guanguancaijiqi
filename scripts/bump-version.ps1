param(
  [Parameter(Mandatory = $true)]
  [ValidateSet("BugFix", "Feature", "NetBaseline")]
  [string]$ReleaseKind,

  [int]$NetBaseline,

  [string]$Version,

  [string]$ChangelogMessage,

  [switch]$NoChangelog
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$configsPath = Join-Path $repoRoot "src\NovelSpider.Config\NovelSpider\Config\Configs.cs"
$utf8NoBom = [System.Text.UTF8Encoding]::new($false)

function Read-Text([string]$Path) {
  [System.IO.File]::ReadAllText($Path)
}

function Write-Text([string]$Path, [string]$Text) {
  [System.IO.File]::WriteAllText($Path, $Text, $utf8NoBom)
}

function Replace-InFile([string]$Path, [string]$Pattern, [string]$Replacement) {
  if (-not (Test-Path -LiteralPath $Path)) {
    return
  }
  $text = Read-Text $Path
  $updated = [regex]::Replace($text, $Pattern, $Replacement)
  if ($updated -ne $text) {
    Write-Text $Path $updated
  }
}

function Get-CurrentVersion() {
  $configsText = Read-Text $configsPath
  $match = [regex]::Match($configsText, 'DisplayVersion\s*=\s*"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)-net(?<net>\d+)-test"')
  if (-not $match.Success) {
    throw "Cannot find Configs.DisplayVersion in $configsPath"
  }
  [PSCustomObject]@{
    Major = [int]$match.Groups["major"].Value
    Minor = [int]$match.Groups["minor"].Value
    Patch = [int]$match.Groups["patch"].Value
    Net = [int]$match.Groups["net"].Value
  }
}

function Parse-Version([string]$Value) {
  $match = [regex]::Match($Value, '^(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)$')
  if (-not $match.Success) {
    throw "Version must use major.minor.patch, for example 10.18.4"
  }
  [PSCustomObject]@{
    Major = [int]$match.Groups["major"].Value
    Minor = [int]$match.Groups["minor"].Value
    Patch = [int]$match.Groups["patch"].Value
  }
}

$current = Get-CurrentVersion

if ($Version) {
  $target = Parse-Version $Version
  $targetNet = if ($NetBaseline -gt 0) { $NetBaseline } else { $target.Major }
} elseif ($ReleaseKind -eq "BugFix") {
  $target = [PSCustomObject]@{
    Major = $current.Major
    Minor = $current.Minor
    Patch = $current.Patch + 1
  }
  $targetNet = $current.Net
} elseif ($ReleaseKind -eq "Feature") {
  $target = [PSCustomObject]@{
    Major = $current.Major
    Minor = $current.Minor + 1
    Patch = 0
  }
  $targetNet = $current.Net
} else {
  if ($NetBaseline -le 0) {
    throw "NetBaseline release requires -NetBaseline, for example -NetBaseline 11"
  }
  $target = [PSCustomObject]@{
    Major = $NetBaseline
    Minor = 0
    Patch = 0
  }
  $targetNet = $NetBaseline
}

$currentSemanticVersion = "$($current.Major).$($current.Minor).$($current.Patch)"
$currentAssemblyVersion = "$currentSemanticVersion.0"
$currentDisplayVersion = "$currentSemanticVersion-net$($current.Net)-test"
$currentTag = "v$currentSemanticVersion-net$($current.Net)"
$currentHeading = "V$currentSemanticVersion"

$semanticVersion = "$($target.Major).$($target.Minor).$($target.Patch)"
$assemblyVersion = "$semanticVersion.0"
$displayVersion = "$semanticVersion-net$targetNet-test"
$targetTag = "v$semanticVersion-net$targetNet"
$heading = "V$semanticVersion Net$targetNet Test"
$shortHeading = "V$semanticVersion"
$targetTagExists = $false

if (Get-Command git -ErrorAction SilentlyContinue) {
  Push-Location $repoRoot
  try {
    $targetTagExists = [bool](git tag --list $targetTag)
  } finally {
    Pop-Location
  }
  if ($targetTagExists) {
    Write-Warning "Target tag already exists: $targetTag. Do not move or reuse it until the historical tag has been reviewed."
  }
}

$activeAssemblyInfoFiles = @(
  "src\NovelSpider\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Config\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Common\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Entity\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Local\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Local.Jieqi\Properties\AssemblyInfo.cs",
  "src\NovelSpider.Target\Properties\AssemblyInfo.cs"
)

foreach ($relativePath in $activeAssemblyInfoFiles) {
  $path = Join-Path $repoRoot $relativePath
  Replace-InFile $path 'AssemblyFileVersion\("\d+\.\d+\.\d+\.\d+"\)' "AssemblyFileVersion(`"$assemblyVersion`")"
  Replace-InFile $path 'AssemblyVersion\("\d+\.\d+\.\d+\.\d+"\)' "AssemblyVersion(`"$assemblyVersion`")"
}

Replace-InFile $configsPath 'DisplayVersion\s*=\s*"\d+\.\d+\.\d+-net\d+-test"' "DisplayVersion = `"$displayVersion`""

$docFiles = @(
  "README.md",
  "MAINTENANCE.md",
  "PROJECT_DEVELOPMENT.md",
  "BRANCH_CONTEXT.md",
  "NET10_MIGRATION_NOTES.md"
)

foreach ($relativePath in $docFiles) {
  $path = Join-Path $repoRoot $relativePath
  if (-not (Test-Path -LiteralPath $path)) {
    continue
  }
  $text = Read-Text $path
  $text = $text.Replace("$currentDisplayVersion / $currentAssemblyVersion", "$displayVersion / $assemblyVersion")
  $text = $text.Replace($currentTag, $targetTag)
  $text = $text.Replace($currentHeading, $shortHeading)
  Write-Text $path $text
}

if (-not $NoChangelog) {
  $changeLogPath = Join-Path $repoRoot "src\NovelSpider\Resources\CHANGELOG.md"
  $changeLogText = Read-Text $changeLogPath
  if (-not $changeLogText.StartsWith($heading)) {
    $date = Get-Date -Format "yyyy-MM-dd"
    $entryLines = @(
      "$heading    $date",
      "- Version bumped to ``$assemblyVersion / $displayVersion``."
    )
    if ($ChangelogMessage) {
      $entryLines += "- $ChangelogMessage"
    }
    $entry = ($entryLines -join [Environment]::NewLine) + [Environment]::NewLine + [Environment]::NewLine
    Write-Text $changeLogPath ($entry + $changeLogText)
  }

  $welcomePath = Join-Path $repoRoot "src\NovelSpider\NovelSpider\WelcomeForm.cs"
  $welcomeText = Read-Text $welcomePath
  if ($welcomeText -notmatch [regex]::Escape($heading)) {
    $date = Get-Date -Format "yyyy-MM-dd"
    $fallbackMessage = if ($ChangelogMessage) { $ChangelogMessage } else { "Updated version metadata." }
    $lines = @(
      ('			"{0}    {1}",' -f $heading, $date),
      "",
      ('			"- Version bumped to {0} / {1}.",' -f $assemblyVersion, $displayVersion),
      "",
      ('			"- {0}",' -f $fallbackMessage),
      "",
      '			"",'
    )
    $insert = ($lines -join [Environment]::NewLine) + [Environment]::NewLine
    $welcomeText = [regex]::Replace(
      $welcomeText,
      '(return string\.Join\(Environment\.NewLine, new string\[\]\s*\r?\n\s*\{\s*\r?\n\s*".*?",\s*\r?\n\s*"\",\s*\r?\n)',
      "`${1}$insert",
      1
    )
    Write-Text $welcomePath $welcomeText
  }
}

[PSCustomObject]@{
  ReleaseKind = $ReleaseKind
  DisplayVersion = $displayVersion
  AssemblyVersion = $assemblyVersion
  TargetTag = $targetTag
  TargetTagExists = $targetTagExists
}
