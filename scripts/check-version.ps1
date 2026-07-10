$ErrorActionPreference = "Stop"
$output = "E:\采集器\ModernizedOutput_Net10_Test"
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
