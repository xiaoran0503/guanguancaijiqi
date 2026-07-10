$ErrorActionPreference = "Stop"
$solution = "E:\采集器\Modernized_Working\NovelSpider.sln"
dotnet restore $solution
dotnet build $solution -c Release -v:minimal
