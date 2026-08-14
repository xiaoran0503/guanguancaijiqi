# NovelSpider Net10 迁移说明

## 当前分支

- 源码目录：`E:\缓存\shipsay\采集器\Modernized_Net10_Git_Working`
- 本地测试输出：`E:\缓存\shipsay\采集器\ModernizedOutput_Net10_Test`
- 迁移来源：`E:\缓存\shipsay\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- 目标框架：`net10.0-windows`
- 平台目标：`x64`
- Runtime Identifier：`win-x64`
- SDK：`global.json` 固定 `10.0.301`
- 已测试运行时：`.NET 10.0.9`
- 当前有效版本：`10.18.5-net10-test / 10.18.5.0`
- 目标标签：`v10.18.5-net10`

## V10.18.5 当前边界

`V10.18.5` 是当前 Net10 维护线，继续沿用现有 Jieqi 写库语义，不启用章节 SQLite 写库缓存。此前的缓存实验已回滚，仍不得恢复。

在没有新的设计、验证计划和可回滚原型之前，不要重新引入以下内容：

- `ChapterWriteBuffer`
- `BookChapterBuffer`
- `IBookChapterBufferProvider`
- `jieqi-chapter-buffer.db3`
- 自动采集界面的章节数据库缓存面板

SQLite 日志模式修复继续保留；章节写库缓存实验不保留。

## 迁移边界

- Net8 仅保留最终基线目录，不从本分支恢复或修改旧 `Modernized_Working`。
- Qiwen 只保留归档源码，不加入 solution，不被主程序引用，不发布 `NovelSpider.Local.Qiwen.dll` 或 `Microsoft.Data.SqlClient.dll`。
- 不修改 XML 规则格式，不引入 JSON 规则副本，不做 DOM V11 规则迁移。
- 不修改 Jieqi 数据库 schema。
- active Net10 solution 只使用 NuGet 稳定版，不使用 beta/preview 包。
- 归档的 `NovelSpider.Local.Qiwen` 不参与 active 依赖审计；未来如果恢复，必须独立迁移和验证 SQL Server。
- 主 WinForms 项目仍保留必要 `.resources`，用于旧窗体 `GetString()` 文本加载；代码不再通过 `GetObject()` 读取旧图标或 toolbar 图片，避免 .NET 10 下 BinaryFormatter 反序列化路径。
- 程序图标使用原生可执行文件图标。active 发布包只发布 `NovelSpider.exe`；NovelAdmin/NovelVip 图标设置只作为归档源码历史保留。
- `Net10RuntimeBootstrap` 不再设置 `ServicePointManager`；.NET 10 使用运行时默认 TLS。初始化只保留编码页注册和正则缓存设置。
- active Net10 solution 固定 Windows-only 和 x64-only：solution configuration 使用 `x64`，active project 设置 `PlatformTarget=x64`，发布使用 `win-x64`。
- GitHub Actions 已覆盖 `net10-v10`、`main` 和 `v10.*-net10` tag，CI 使用仓库相对路径脚本和 `runtime\Rules` / `runtime\Tasks` 种子数据。

## 依赖审计

截至 2026-07-09 的 Net10 依赖审计，active solution 没有稳定版依赖可升级，也没有 NuGet 报告的漏洞包。

当前 active 稳定依赖基线：

- `MySqlConnector 2.6.1`
- active 代码使用内置 `System.Text.Json`；`Newtonsoft.Json 13.0.4` 仍作为 jieba.NET 的直接稳定传递依赖覆盖保留。
- `System.Data.SQLite.Core 1.0.119`
- `SharpZipLib 1.4.2`
- `CHSPinYinConv 1.0.0`
- `jieba.NET 0.42.2`
- `DockPanelSuite 3.1.1`
- `DockPanelSuite.ThemeVS2015 3.1.1`
- `System.Management 10.0.9`
- `Microsoft.Extensions.DependencyInjection.Abstractions 10.0.9`
- `Microsoft.Extensions.Logging.Abstractions 10.0.9`

明确排除的预览/测试版本：

- `Newtonsoft.Json 13.0.5-beta1`
- `Microsoft.Data.SqlClient 7.1.0-preview1.*`
- `.NET 11 preview` 包，例如 `System.Management 11.0.0-preview.*` 和 `Microsoft.Extensions.* 11.0.0-preview.*`

SQL Server 依赖边界：

- active Net10 solution 和发布包不引用 `System.Data.SqlClient` 或 `Microsoft.Data.SqlClient`。
- `NovelSpider.Local.Qiwen` 作为归档源码保留历史 `Microsoft.Data.SqlClient 6.1.1` 引用，但不纳入 active dependency modernization。
- 如果将来恢复 Qiwen，必须单独迁移到 `net10.0-windows`，更新到当时最新稳定 SQL Server 驱动，恢复 solution/publish 覆盖，并重新验证 SQL Server 行为。

## 本机运行说明

当前机器 Windows 版本为 `10.0.19045`。本机可用于迁移测试，但最终发布验证建议在官方支持的 Windows 11、Windows Server 或 LTSC 环境重复执行。

参考：`https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/supported-os.json`
