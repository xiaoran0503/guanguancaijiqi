$ErrorActionPreference = "Stop"
$solution = "E:\采集器\Modernized_Net10_Working\NovelSpider.sln"
dotnet list $solution package --vulnerable --include-transitive
