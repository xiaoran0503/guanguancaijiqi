$ErrorActionPreference = "Stop"
$solution = "E:\采集器\Modernized_Net10_Working\NovelSpider.sln"
dotnet restore $solution
dotnet build $solution -c Release -p:Platform=x64 --no-restore -v:minimal
