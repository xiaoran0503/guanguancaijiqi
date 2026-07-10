param(
  [string]$Configuration = "Release",
  [string]$Output = ""
)

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path

if ([string]::IsNullOrWhiteSpace($Output)) {
  $Output = Join-Path $repoRoot "artifacts\publish\NovelSpider-Net8"
}

$publishDir = [System.IO.Path]::GetFullPath($Output)
$projects = @(
  "src\NovelSpider\NovelSpider.csproj",
  "src\NovelAdmin\NovelAdmin.csproj",
  "src\NovelVip\NovelVip.csproj"
)

if (Test-Path -LiteralPath $publishDir) {
  Remove-Item -LiteralPath $publishDir -Recurse -Force
}

New-Item -ItemType Directory -Path $publishDir | Out-Null

foreach ($project in $projects) {
  $projectPath = Join-Path $repoRoot $project
  dotnet publish $projectPath -c $Configuration -o $publishDir --no-restore
}

$retiredFiles = @(
  "NovelSpider.Local.Qiwen.dll",
  "NovelSpider.Local.Qiwen.pdb",
  "Microsoft.Data.SqlClient.dll"
)

foreach ($file in $retiredFiles) {
  $path = Join-Path $publishDir $file
  if (Test-Path -LiteralPath $path) {
    Remove-Item -LiteralPath $path -Force
  }
}

$requiredFiles = @("NovelSpider.exe", "NovelAdmin.exe", "NovelVip.exe")

foreach ($file in $requiredFiles) {
  $path = Join-Path $publishDir $file
  if (-not (Test-Path -LiteralPath $path)) {
    throw "Missing published file: $file"
  }

  $item = Get-Item -LiteralPath $path
  [PSCustomObject]@{
    File = $file
    ProductVersion = $item.VersionInfo.ProductVersion
    FileVersion = $item.VersionInfo.FileVersion
  }
}

Write-Host "Published to $publishDir"
