$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$output = if ($env:NOVELSPIDER_PUBLISH_DIR) {
  $env:NOVELSPIDER_PUBLISH_DIR
} else {
  Join-Path $repoRoot "artifacts\NovelSpider-Net10-win-x64"
}
$files = @("NovelSpider.exe", "NovelAdmin.exe", "NovelVip.exe")
foreach ($file in $files) {
  $path = Join-Path $output $file
  if (-not (Test-Path -LiteralPath $path)) {
    throw "缺少文件：$path"
  }
  $item = Get-Item -LiteralPath $path
  [PSCustomObject]@{
    File = $file
    ProductVersion = $item.VersionInfo.ProductVersion
    FileVersion = $item.VersionInfo.FileVersion
  }
}
