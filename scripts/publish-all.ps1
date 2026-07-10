$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$output = if ($env:NOVELSPIDER_PUBLISH_DIR) {
  $env:NOVELSPIDER_PUBLISH_DIR
} else {
  Join-Path $repoRoot "artifacts\NovelSpider-Net10-win-x64"
}
$seedDataRoot = Join-Path $repoRoot "runtime"
$fallbackDataRoot = "E:\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1"
$runtimeDataDirs = @("Rules", "Tasks")
$backupRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("NovelSpiderPublishData_" + [guid]::NewGuid().ToString("N"))
$projects = @(
  (Join-Path $repoRoot "src\NovelSpider\NovelSpider.csproj"),
  (Join-Path $repoRoot "src\NovelAdmin\NovelAdmin.csproj"),
  (Join-Path $repoRoot "src\NovelVip\NovelVip.csproj")
)
if (-not (Test-Path -LiteralPath $output)) {
  New-Item -ItemType Directory -Path $output -Force | Out-Null
}
New-Item -ItemType Directory -Path $backupRoot | Out-Null
foreach ($dir in $runtimeDataDirs) {
  $current = Join-Path $output $dir
  if (Test-Path -LiteralPath $current) {
    Copy-Item -LiteralPath $current -Destination (Join-Path $backupRoot $dir) -Recurse
  }
}
foreach ($project in $projects) {
  dotnet publish $project -c Release -p:Platform=x64 -r win-x64 --self-contained false -o $output --no-restore
}
$retiredFiles = @(
  "NovelSpider.Local.Qiwen.dll",
  "NovelSpider.Local.Qiwen.pdb",
  "Microsoft.Data.SqlClient.dll",
  "System.Data.SqlClient.dll"
)
foreach ($file in $retiredFiles) {
  $retiredPath = Join-Path $output $file
  if (Test-Path -LiteralPath $retiredPath) {
    Remove-Item -LiteralPath $retiredPath -Force
  }
}
foreach ($dir in $runtimeDataDirs) {
  $target = Join-Path $output $dir
  $backup = Join-Path $backupRoot $dir
  $seed = Join-Path $seedDataRoot $dir
  $fallback = if (Test-Path -LiteralPath $fallbackDataRoot) {
    Join-Path $fallbackDataRoot $dir
  } else {
    $null
  }
  if (Test-Path -LiteralPath $backup) {
    if (Test-Path -LiteralPath $target) {
      Remove-Item -LiteralPath $target -Recurse -Force
    }
    Copy-Item -LiteralPath $backup -Destination $target -Recurse
  } elseif (Test-Path -LiteralPath $seed) {
    if (Test-Path -LiteralPath $target) {
      Remove-Item -LiteralPath $target -Recurse -Force
    }
    Copy-Item -LiteralPath $seed -Destination $target -Recurse
  } elseif (-not (Test-Path -LiteralPath $target) -and $fallback -and (Test-Path -LiteralPath $fallback)) {
    Copy-Item -LiteralPath $fallback -Destination $target -Recurse
  } elseif (-not (Test-Path -LiteralPath $target)) {
    New-Item -ItemType Directory -Path $target | Out-Null
  }
}
$changeLogSource = Join-Path $repoRoot "src\NovelSpider\Resources\CHANGELOG.md"
$changeLogTargetDir = Join-Path $output "Resources"
if (Test-Path -LiteralPath $changeLogSource) {
  New-Item -ItemType Directory -Path $changeLogTargetDir -Force | Out-Null
  Copy-Item -LiteralPath $changeLogSource -Destination (Join-Path $changeLogTargetDir "CHANGELOG.md") -Force
}
Remove-Item -LiteralPath $backupRoot -Recurse -Force
