$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $repoRoot "NovelSpider.sln"
dotnet restore $solution
dotnet build $solution -c Release -p:Platform=x64 --no-restore -v:minimal
