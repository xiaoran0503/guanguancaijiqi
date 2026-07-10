$ErrorActionPreference = "Stop"
$output = "E:\采集器\ModernizedOutput"
$fallbackDataRoot = "E:\采集器\ModernizedOutput_V8.10.3_Milestone"
$runtimeDataDirs = @("Rules", "Tasks")
$backupRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("NovelSpiderPublishData_" + [guid]::NewGuid().ToString("N"))
$projects = @(
  "E:\采集器\Modernized_Working\src\NovelSpider\NovelSpider.csproj",
  "E:\采集器\Modernized_Working\src\NovelAdmin\NovelAdmin.csproj",
  "E:\采集器\Modernized_Working\src\NovelVip\NovelVip.csproj"
)
if (-not (Test-Path -LiteralPath $output)) {
  New-Item -ItemType Directory -Path $output | Out-Null
}
New-Item -ItemType Directory -Path $backupRoot | Out-Null
foreach ($dir in $runtimeDataDirs) {
  $current = Join-Path $output $dir
  if (Test-Path -LiteralPath $current) {
    Copy-Item -LiteralPath $current -Destination (Join-Path $backupRoot $dir) -Recurse
  }
}
foreach ($project in $projects) {
  dotnet publish $project -c Release -o $output --no-restore
}
$retiredFiles = @(
  "NovelSpider.Local.Qiwen.dll",
  "NovelSpider.Local.Qiwen.pdb",
  "Microsoft.Data.SqlClient.dll"
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
  $fallback = Join-Path $fallbackDataRoot $dir
  if (Test-Path -LiteralPath $backup) {
    if (Test-Path -LiteralPath $target) {
      Remove-Item -LiteralPath $target -Recurse -Force
    }
    Copy-Item -LiteralPath $backup -Destination $target -Recurse
  } elseif (-not (Test-Path -LiteralPath $target) -and (Test-Path -LiteralPath $fallback)) {
    Copy-Item -LiteralPath $fallback -Destination $target -Recurse
  }
}
Remove-Item -LiteralPath $backupRoot -Recurse -Force
