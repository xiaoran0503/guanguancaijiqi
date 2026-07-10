$ErrorActionPreference = "Stop"
$solution = "E:\采集器\Modernized_Working\NovelSpider.sln"
dotnet list $solution package --vulnerable --include-transitive
