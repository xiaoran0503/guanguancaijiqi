$ErrorActionPreference = "Stop"
$repoRoot = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $repoRoot "NovelSpider.sln"
dotnet list $solution package --vulnerable --include-transitive
