# NovelSpider 现代化项目开发文档

## 1. 文档目的

本文档用于记录 `NovelSpider` 现代化版本的项目结构、构建发布方式、版本迭代规则、关键改造点和后续开发注意事项。

当前目录是从 `V8.17.1 / 8.17.1.0` 的 `.NET 8 / net8.0-windows` 最终基线复制出的 `.NET 10 / net10.0-windows` 迁移测试分支。Net8 维护仍只在 `Modernized_Working` 进行，不要把本目录的迁移改动回写到 Net8 分支。

当前工作目录：

```text
E:\采集器\Modernized_Net10_Working
```

当前测试输出目录：

```text
E:\采集器\ModernizedOutput_Net10_Test
```

V8.0、V8.2、V8.4、V8.5、V8.6、V8.7、V8.8、V8.10.3、V8.13.3、V8.17.1 已作为里程碑版本封存。本分支先验证 Net10 可编译、可发布、可启动和核心采集/Jieqi 流程可用。

## 2. 版本与目录约定

| 类型 | 路径 | 说明 |
| --- | --- | --- |
| V8.0 里程碑源码 | `E:\采集器\Modernized_V8.0_Milestone` | 已封存，不再直接修改 |
| V8.0 里程碑运行包 | `E:\采集器\ModernizedOutput_V8.0_Milestone` | 可回退、可对照测试的稳定包 |
| V8.5 里程碑源码 | `E:\采集器\Modernized_V8.5_Milestone` | 已封存，不再直接修改 |
| V8.5 里程碑运行包 | `E:\采集器\ModernizedOutput_V8.5_Milestone` | 异常堆栈保真修复稳定包 |
| V8.6 里程碑源码 | `E:\采集器\Modernized_V8.6_Milestone` | 已封存，不再直接修改 |
| V8.6 里程碑运行包 | `E:\采集器\ModernizedOutput_V8.6_Milestone` | 低风险过时 API 收敛稳定包 |
| V8.7 里程碑源码 | `E:\采集器\Modernized_V8.7_Milestone` | 已封存，不再直接修改 |
| V8.7 里程碑运行包 | `E:\采集器\ModernizedOutput_V8.7_Milestone` | SQL Server 驱动现代化稳定包 |
| V8.13.3 PreV8.14 里程碑源码 | `E:\采集器\Modernized_V8.13.3_PreV8.14_Milestone` | V8.14-V8.17 前的稳定回退点 |
| V8.13.3 PreV8.14 里程碑运行包 | `E:\采集器\ModernizedOutput_V8.13.3_PreV8.14_Milestone` | V8.13.3 可用运行包回退点 |
| Net8 最终基线源码 | `E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1` | V8.17.1 Net8 最终源码基线，只读保存 |
| Net8 最终基线运行包 | `E:\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1` | V8.17.1 Net8 最终运行包，只读保存 |
| Net8 维护源码 | `E:\采集器\Modernized_Working` | Net8 稳定维护分支，不接收 Net10 迁移改动 |
| Net10 迁移源码 | `E:\采集器\Modernized_Net10_Working` | 当前 Net10 测试分支 |
| Net10 测试输出 | `E:\采集器\ModernizedOutput_Net10_Test` | 当前 Net10 发布测试包 |

版本规则：

- `8.0` 是 .NET 8 现代化后的初始里程碑版本。
- 小功能和修复可使用 `8.0.1`、`8.0.2`。
- 较大功能迭代可使用 `8.1`、`8.2`。
- 程序界面显示版本使用 `Configs.DisplayVersion`。
- 文件属性版本使用各项目 `Properties\AssemblyInfo.cs` 中的 `AssemblyVersion` 和 `AssemblyFileVersion`。
- 每次版本变化都要同步更新 `src\NovelSpider\Resources\CHANGELOG.md` 中的“更新日志”。
- Net8 维护分支不得升级 `TargetFramework`；Net10 迁移必须使用独立工作区，例如 `E:\采集器\Modernized_Net10_Working`。

## 3. 技术栈

| 项目项 | 当前值 |
| --- | --- |
| 运行框架 | `.NET 10 Windows / net10.0-windows` |
| UI 框架 | Windows Forms |
| 主界面停靠组件 | DockPanelSuite `3.1.1` |
| 主程序主题包 | DockPanelSuite.ThemeVS2015 `3.1.1` |
| MySQL 驱动 | MySqlConnector `2.6.1` |
| SQL Server 驱动 | 已从主程序移除，Qiwen 源码仅归档保留 |
| SQLite 驱动 | System.Data.SQLite.Core `1.0.119` |
| 压缩库 | SharpZipLib `1.4.2` |
| 拼音库 | CHSPinYinConv `1.0.0` |
| 分词实现 | jieba.NET `0.42.2`，通过 `JiebaTextSegmenter` 提供 Net10 原生命名调用 |
| Windows 系统信息 | System.Management `10.0.9` |

Net10 迁移测试分支补充：

- 当前 active solution 目标框架为 `.NET 10 / net10.0-windows`，SDK 固定为 `10.0.301`。
- 当前 Net10 测试版本为 `10.4.4-net10-test / 10.4.4.0`，发布平台固定为 Windows-only `win-x64` / `x64`。
- GitHub Actions 已加入 `net10-v10` 分支：推送 `net10-v10` / `main` 自动构建并上传 Windows x64 artifact，推送 `v10.*-net10` tag 自动创建 GitHub Release。
- active solution 依赖采用 NuGet 稳定最新版策略；截至 2026-07-09，`MySqlConnector 2.6.1`、`Newtonsoft.Json 13.0.4`、`System.Data.SQLite.Core 1.0.119`、DockPanelSuite `3.1.1`、SharpZipLib `1.4.2`、CHSPinYinConv `1.0.0`、jieba.NET `0.42.2`、`System.Management 10.0.9` 和 `Microsoft.Extensions.* 10.0.9` 均无 stable 更新。
- 不采用 beta/preview 包，例如 `Newtonsoft.Json 13.0.5-beta1`、`Microsoft.Data.SqlClient 7.1.0-preview1.*` 或 `.NET 11 preview` 包。
- active solution 和发布包不包含 `System.Data.SqlClient` / `Microsoft.Data.SqlClient`；`NovelSpider.Local.Qiwen` 仍为归档源码，不参与 active dependency audit。

开发机器需要安装：

- Windows 10/11
- .NET 8 SDK
- Windows Desktop Runtime 8，如果只运行发布包则至少需要运行时

## 4. 解决方案结构

解决方案文件：

```text
E:\采集器\Modernized_Working\NovelSpider.sln
```

主要项目：

| 项目 | 类型 | 说明 |
| --- | --- | --- |
| `NovelSpider` | WinExe | 主采集器程序，入口为 `NovelSpider.exe` |
| `NovelAdmin` | Archived WinExe | V10.3.0 起 active 下架，源码保留不构建不发布 |
| `NovelVip` | Archived WinExe | V10.3.0 起 active 下架，源码保留不构建不发布 |
| `NovelSpider.Config` | Library | 全局配置、基础配置、任务配置、版本信息 |
| `NovelSpider.Common` | Library | 公共工具、文本处理、拼音、压缩、SQLite、授权兼容逻辑 |
| `NovelSpider.Entity` | Library | 实体模型，例如小说、章节、规则等数据结构 |
| `NovelSpider.Target` | Library | 发布目标、页面生成和目标站适配逻辑 |
| `NovelSpider.Local` | Library | 本地 CMS 通用适配层 |
| `NovelSpider.Local.Jieqi` | Library | 杰奇 CMS 适配层 |
| `NovelSpider.Local.Qiwen` | Archived Library | 奇文 CMS 归档源码，主程序和解决方案不再引用 |

## 5. 程序入口

| 程序 | 入口文件 | 说明 |
| --- | --- | --- |
| 主程序 | `src\NovelSpider\NovelSpider\Program.cs` | 加载配置，开放本地功能，启动 `MdiForm` |
| 管理台 | `src\NovelAdmin\NovelAdmin\Program.cs` | 归档源码，不在 active solution 中构建 |
| 高级工具 | `src\NovelVip\NovelVip\Program.cs` | 归档源码，不在 active solution 中构建 |

主程序启动流程简述：

1. 读取命令行参数。
2. 调用 `Configs.LoadConfigs()` 加载 `BaseConfig.xml` 和 `TaskConfig.xml`。
3. 设置本地功能默认可用。
4. 启动 `MdiForm`。
5. `MdiForm` 初始化 DockPanel 主界面并打开“更新日志”页面。

## 6. 配置文件

运行目录中会使用以下配置文件：

| 文件 | 说明 |
| --- | --- |
| `BaseConfig.xml` | 站点、数据库、CMS、全局采集配置 |
| `TaskConfig.xml` | 采集任务配置 |
| `Key.License` / `Key.Machine` | 旧版兼容文件，现代化版本默认本地开放 |

配置加载逻辑位于：

```text
src\NovelSpider.Config\NovelSpider\Config\Configs.cs
```

注意事项：

- `Configs.DisplayVersion` 控制界面显示版本。
- `Configs.Build` 控制更新日志页显示的 Build 日期。
- `BaseConfigInfo` 中授权相关字段目前默认开放，用于兼容旧界面判断。

## 7. UI 结构说明

主窗口：

```text
src\NovelSpider\MdiForm.cs
```

关键点：

- 使用 DockPanelSuite 作为文档停靠容器。
- 主题通过 `DockThemeFactory` 创建，当前使用 `VS2015BlueTheme`。
- 子页面统一通过 `ShowDockContent(DockContent content)` 打开。
- 不再使用旧式 `MdiParent = this` 方式承载 DockContent，避免 .NET 8 下 DockPanel ActiveContent 异常。
- `dockPanel` 顶部需要给菜单栏留出空间，避免文档标签栏被遮挡。

更新日志页面：

```text
src\NovelSpider\NovelSpider\WelcomeForm.cs
```

维护规则：

- 页面标题为“更新日志”。
- 更新内容优先读取 `src\NovelSpider\Resources\CHANGELOG.md`，文件缺失时才使用 `WelcomeForm.GetChangeLogText()` 内置兜底文本。
- 每次版本迭代时，在 `CHANGELOG.md` 顶部新增版本记录。
- 不建议再依赖旧 `.resx` 中的大段消息文本。

## 8. 授权与功能开放状态

V8.0 起，本地现代化版本默认开放普通功能和高级功能。

相关位置：

| 文件 | 说明 |
| --- | --- |
| `src\NovelSpider\NovelSpider\Program.cs` | 主程序启动时设置 `LicenseOk`、`LicenseVip` 等状态 |
| `src\NovelSpider.Config\NovelSpider\Config\BaseConfigInfo.cs` | 配置默认值中授权相关字段默认开放 |
| `src\NovelSpider.Common\NovelSpider\Common\FormatDateTime.cs` | `CheckHost()` 默认返回 true，兼容旧验证调用 |
| `src\NovelVip\NovelVip\form1.cs` | 高级工具显示普通和高级功能可用 |

注意：旧代码里仍可能存在 `LicenseOk`、`LicenseVip` 判断，这是为了减少重构风险保留的兼容判断。当前默认值为开放状态。

## 9. 旧 DLL 替换与兼容层

V8.0 现代化过程中，尽量避免直接依赖旧 DLL。

| 旧组件 | 当前处理 |
| --- | --- |
| `ChnCharInfo.dll` | 使用 NuGet 包 `CHSPinYinConv` |
| `PanGu.dll` | 已移除旧命名空间外壳，使用 `jieba.NET` 与 `JiebaTextSegmenter.cs` |
| `Sunrise.Spell.dll` | 不再作为核心依赖，后续如需要拼写纠错应另选现代化方案 |

兼容层位置：

```text
src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\JiebaTextSegmenter.cs
```

拼音相关位置：

```text
src\NovelSpider.Common\NovelSpider\Common\FormatText.cs
src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\CHz2Py.cs
```

## 10. 构建命令

在任意目录可执行：

```powershell
dotnet build "E:\采集器\Modernized_Working\NovelSpider.sln" -c Release -v:minimal
```

当前主线要求 Release 构建保持：

```text
0 个警告
0 个错误
```

若出现真实 API 过时或安全警告，优先改代码；WinForms/反编译遗留噪音只允许通过根级 `Directory.Build.props` 集中抑制。

## 11. 发布命令

发布到当前运行目录：

```powershell
$out = "E:\采集器\ModernizedOutput"
$projects = @(
  "E:\采集器\Modernized_Working\src\NovelSpider\NovelSpider.csproj",
)
foreach ($project in $projects) {
  dotnet publish $project -c Release -o $out -v:quiet -p:WarningLevel=0
  if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
```

发布后应检查：

```powershell
Get-ChildItem -LiteralPath "E:\采集器\ModernizedOutput" -Include NovelSpider.exe -File |
  ForEach-Object { $_.VersionInfo.FileVersion }
```

## 12. 手工冒烟测试清单

每次发布后至少测试：

1. 双击 `E:\采集器\ModernizedOutput\NovelSpider.exe`。
2. 主窗口能正常打开，不出现 DockPanel 主题错误。
3. 顶部菜单没有遮挡文档标签栏。
4. 默认打开“更新日志”页面。
5. 标题栏显示 `V8.1` 或当前迭代版本。
6. 点击菜单中的“更新日志”，页面能重复打开或聚焦。
7. 双击 `NovelAdmin.exe`，确认窗口能打开。
8. 双击 `NovelVip.exe`，确认普通功能和高级功能显示可用。
9. 如改动采集逻辑，至少打开自动采集、手动采集、规则、配置页面确认不崩。

## 13. 开发原则

- 优先保持原版 UI 和操作习惯，不做大幅重设计。
- 优先修根因，不做只掩盖异常的补丁。
- 旧反编译代码命名混乱，非必要不要大规模重命名，避免扩大风险。
- 修改 WinForms 设计器代码时要小心控件初始化顺序。
- DockPanel 子窗体统一继承 `DockContent` 并通过 `ShowDockContent` 打开。
- 新功能优先加在 `Modernized_Working`，不要修改 `Modernized_V8.0_Milestone`。
- 发布前必须重新编译，并确认输出目录里的 exe 时间和版本已更新。

## 14. 版本迭代流程

建议流程：

1. 在 `Modernized_Working` 修改代码。
2. 更新 `Configs.DisplayVersion` 和相关 `AssemblyInfo.cs`。
3. 在 `src\NovelSpider\Resources\CHANGELOG.md` 顶部新增更新日志。
4. 执行 Release 构建。
5. 发布到 `E:\采集器\ModernizedOutput`。
6. 手工冒烟测试三个 exe。
7. 如果该版本稳定，再复制封存为新的里程碑目录，例如：

```text
E:\采集器\Modernized_V8.1_Milestone
E:\采集器\ModernizedOutput_V8.1_Milestone
```


## 15. V8.1 数据库兼容记录

V8.1 新增事项：

- 数据库连接测试可识别 `MySQL`、`MariaDB`、`Percona Server`。
- MySQL 兼容范围明确为 `5.7.x`、`8.x`、`9.x`。
- MySQL `8.x/9.x` 自动补齐推荐连接参数：`Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True`。
- MySQL `5.7.x` 使用保守兼容策略，不强制修改用户连接串。
- MariaDB `10.x/11.x` 自动补齐 `Charset=utf8mb4;SslMode=Preferred`。
- Percona Server `5.7.x/8.x` 走 MySQL 协议兼容策略。
- 连接测试弹窗显示服务器类型、版本、版本说明、字符集、排序规则和 SQL Mode。
- 依赖升级：`MySqlConnector 2.6.1`、`Newtonsoft.Json 13.0.4`、`System.Data.SqlClient 4.9.1`。
- 显式覆盖 `jieba.NET` 带入的旧传递依赖，消除 `System.Drawing.Common 4.7.0` 高危漏洞链。

数据库兼容矩阵：

| 数据库 | 兼容版本 | 策略 |
| --- | --- | --- |
| MySQL | `5.7.x` | 保守兼容，不强制改连接串 |
| MySQL | `8.x / 9.x` | 自动补齐 `utf8mb4`、`Preferred SSL`、`AllowPublicKeyRetrieval` |
| MariaDB | `10.x / 11.x` | 自动补齐 `utf8mb4`、`Preferred SSL` |
| Percona Server | `5.7.x` | 保守兼容 |
| Percona Server | `8.x` | 按 MySQL 8 策略自动补齐推荐参数 |

## 16. V8.17.1 性能跟踪关闭记录

V8.17.1 热修事项：

- 版本升级为 `8.17.1.0 / 8.17.1`，继续使用 `.NET 8 Windows WinForms` 主线。
- `PerformanceTelemetry` 默认关闭，`Measure()` 返回 no-op，`Record()` 直接返回，不再默认创建 `Log\Performance\yyyyMMdd.csv`。
- 需要临时排障时，在启动程序前设置环境变量 `NOVELSPIDER_PERFORMANCE=1`、`true`、`on` 或 `yes`，即可恢复 CSV 跟踪。
- 根据 `E:\采集器\20260709.csv`，254 章采集总计约 68.3 秒，章节平均约 269ms；MySQL 总计约 2.9 秒、P95 约 3ms；TXT 写入总计约 0.84 秒、P95 约 14ms。
- 后续性能优化优先看章节整体流程中的规则解析、模板处理和章节页生成；MySQL/TXT 单章写入当前不是主要瓶颈。
- 网络层继续保持 HTTP/1.1 兼容优先，不恢复强制 HTTP/2。

## 17. V8.17 net8 安全、性能与可维护性记录

V8.17 新增事项：

- 已封存 V8.13.3 可用基线：`E:\采集器\Modernized_V8.13.3_PreV8.14_Milestone`、`E:\采集器\ModernizedOutput_V8.13.3_PreV8.14_Milestone`。
- 版本升级为 `8.17.0.0 / 8.17`，继续使用 `.NET 8 Windows WinForms` 主线，不做 net10 升级。
- Qiwen 适配器从主线下线：配置页只保留 Jieqi，旧 Qiwen 配置自动切回 Jieqi，`NovelSpider.sln` 和主程序发布不再引用 Qiwen 项目；Qiwen 源码仅归档保留。
- `InsertNovel` 改为参数化 SQL + 单连接事务，并通过 `LAST_INSERT_ID()` 获取新增书号，避免按书名/作者二次拼接查询。
- 参数化 `GetChapterInfo`、章节列表、章节页前后章查询、WAP 目录查询和 SQLite 采集日志写入热点。
- 管理台保留手写 SQL 面板，但执行危险 SQL 前会二次确认；自动生成的 ID 列表和关键词查询会清理输入。
- `ChapterFileWriter` 扩展为通用文本原子写入入口，全集 TXT、JAR 分片、MANIFEST 和部分目录/OPF/全集 HTML 写入走临时文件替换。
- 封面保存集中到 `CoverImageService`，减少 `System.Drawing` 在 Jieqi 业务逻辑中的散落调用，为未来 net10 分支做准备。
- 本轮不改变 XML 规则格式、不默认启用内存延迟写库、不移除 Qiwen 源码目录。

## 17. V8.13 net8 性能稳定、现代化与可维护性记录

V8.13.3 热修事项：

- 进一步恢复 HTTP 下载路径到 V8.10.3 行为，移除网络层请求耗时插桩，优先保证最新列表和小说页规则兼容。
- `HttpTransportPool.Send()` 重新保持“发送请求后直接返回响应”的旧行为，不在响应返回前写性能日志。
- `HttpClient.GetStringWork()` / `GetImageWork()` 不再包裹 HTTP 性能统计，避免对老站点、反代站采集时序产生干扰。
- 保留 GBK 编码页注册、Rules/Tasks 发布保护、MySQL/TXT 性能统计和章节按域名延时。

V8.13.2 热修事项：

- 回滚 V8.13 中对采集请求强制优先 HTTP/2 的激进优化，恢复 V8.10.3 默认 HTTP/1.1 协商行为。
- 保留 `HttpTransportPool` 连接池、TLS 复用和性能统计，但不再设置 `HttpRequestMessage.Version = 2.0` / `VersionPolicy`，避免部分小说站、反代站返回内容和 HTTP/1.1 不一致。
- 移除 `SocketsHttpHandler` 的 HTTP/2 keep-alive ping 相关设置，降低规则列表页正则匹配失败风险。

V8.13.1 热修事项：

- 修复规则测试和采集规则使用 `gbk` / `gb2312` 编码时，.NET 8 未注册代码页导致 `'gbk' is not a supported encoding name` 的问题。
- `Net10RuntimeBootstrap.Initialize()` 现在同时注册 `CodePagesEncodingProvider.Instance`，并继续设置系统默认 TLS 与正则缓存。
- `NovelSpider.Program` 在 `Configs.LoadConfigs()` 前执行网络/编码兼容初始化。
- `FormatText.GetCharset()` 增加编码注册兜底；当 CMS 编码配置为 `gbk` 时明确返回 `Encoding.GetEncoding("gbk")`。
- `Page` 和公共 `HttpClient` 增加类内初始化兜底，避免规则测试或独立调用绕过启动初始化。
- `scripts\publish-all.ps1` 发布前会备份当前运行目录 `Rules` / `Tasks` 并在发布后恢复；当前输出目录缺失时会从 V8.10.3 里程碑补齐，避免采集运行包缺少规则任务。

V8.13 新增事项：

- 已在进入 V8.11/V8.12/V8.13 阶段前额外封存 V8.10.3 快照：`E:\采集器\Modernized_V8.10.3_PreV8.11_Milestone`、`E:\采集器\ModernizedOutput_V8.10.3_PreV8.11_Milestone`。
- 版本升级为 `8.13.0.0 / 8.13`，继续使用 `.NET 8 Windows WinForms` 稳定基线，不再继续 V8.9 的 `.NET 10` 失败线。
- 新增 `PerformanceTelemetry`，运行时会在 `Log\Performance\yyyyMMdd.csv` 记录 `http`、`mysql`、`file`、`collect` 等热路径耗时，便于后续定位性能瓶颈。
- `HttpTransportPool` 增强 HTTP/2 自动协商、连接保活探测和请求耗时统计，继续保持同步 `GetStringWork()`、代理、Cookie、编码和规则解析行为。
- `Page.GetChapterInfo()` 改为按域名执行 `ChapterUrlWait` 毫秒级节流，避免多个 `Page` 实例绕过章节延时；设置 `5000` 仍表示同域章节正文请求至少间隔 5 秒。
- 新增 `ChapterFileWriter`，章节 TXT 写入改为大缓冲临时文件 + 原子替换，降低半截文件风险。
- `MySqlHelper` 统一记录 MySQL 执行耗时；`UpdateChapterOrder` 和 `UpdateNovel` 热点路径改为参数化 SQL。
- 更新日志迁移为独立 Markdown 资源 `src\NovelSpider\Resources\CHANGELOG.md`，发布时复制到运行目录 `Resources\CHANGELOG.md`。
- 新增 `MAINTENANCE.md`、`PERFORMANCE_BASELINE.md` 和 `scripts` 维护脚本目录，为后续开发、构建、发布、排错提供固定入口。

维护入口：

| 文件/目录 | 用途 |
| --- | --- |
| `MAINTENANCE.md` | 常见修改入口、性能排查、发布前检查 |
| `PERFORMANCE_BASELINE.md` | 30章/100章性能基准记录模板 |
| `scripts\build-release.ps1` | restore + Release 构建 |
| `scripts\check-vulnerable.ps1` | 漏洞包检查 |
| `scripts\publish-all.ps1` | 发布三套程序到 `ModernizedOutput` |
| `scripts\check-version.ps1` | 检查三套 EXE 文件版本 |

## 17. V8.10 net8 稳定基线现代化与性能优化记录

V8.10.3 热修事项：

- 已封存 V8.10.3 稳定源码和可运行包：`E:\采集器\Modernized_V8.10.3_Milestone`、`E:\采集器\ModernizedOutput_V8.10.3_Milestone`。
- 修复 HTTPS 连接池提速后，自动采集章节延时在部分分支中不稳定生效的问题。
- `NovelSpider.Target.Page.GetChapterInfo()` 入口统一执行 `TaskConfigInfo.ChapterUrlWait` 毫秒级最小间隔节流，保证每次章节正文请求都经过同一限速点。
- 保留原有任务配置单位为毫秒，设置 `5000` 表示至少间隔 5 秒请求下一章正文。

V8.10.2 热修事项：

- 修复 MySQL 8/9 默认 `caching_sha2_password` 认证在未补齐连接串时连接失败的问题。
- `DatabaseConnectionProfile.Detect()`、配置页“测试数据库”和保存配置路径都会先规范化连接串，缺省补齐 `Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True`。
- 配置页 `NumericUpDown` 读取旧配置数值时会自动限制到控件允许范围，避免旧配置中的大数值导致设置页崩溃。

V8.10.1 热修事项：

- 修复旧 `BaseConfig.xml` 缺少 `isboyCorresponding`、`LagerSortCorresponding`、`SmallSortCorresponding` 等字段时，打开“系统设置/配置”页面在 `ConfigForm.initFormData()` 中空引用崩溃的问题。
- `BaseConfigInfo.EnsureDefaults()` 会在配置加载后补齐缺失字符串和关键默认值，避免旧配置文件阻断 UI 初始化。
- `ConfigForm` 读取对应表、日志选择、目录规则等字符串时增加空值兜底。

V8.10 新增事项：

- 已将 V8.9/.NET 10 失败线归档为 `E:\采集器\Modernized_V8.9_Failed_Net10`、`E:\采集器\ModernizedOutput_V8.9_Failed_Net10`，主线不再继续该方向。
- 已从 V8.8 可信里程碑恢复 `E:\采集器\Modernized_Working`，继续以 `.NET 8 Windows WinForms` 作为稳定基线。
- 版本升级为 `8.10.0.0 / 8.10`，发布输出仍覆盖到 `E:\采集器\ModernizedOutput`。
- 保留自动采集窗口稳定性兜底：`Rules`、`Tasks`、`Log` 目录不存在时返回空数组；配置路径为空或文件不存在时返回默认配置对象；规则/方案下拉空选择时跳过加载。
- HTTPS 采集底层恢复为 net8 池化传输层，按代理配置复用连接和 TLS 握手，同时保留同步 `GetStringWork()`、POST、Referer、UA、Cookie、代理、编码和规则解析行为。
- Jieqi 写库继续不写入 `jieqi_article_chapter.summary`，并把更新章节、刷新最后章节等热点路径扩展为单连接事务和参数化 SQL。
- 统一章节 TXT 写入辅助，减少重复目录检查和散落 `StreamWriter` 写入代码。
- 启动时继续初始化系统默认 TLS 与正则缓存，保持构建 `0` 错误、`0` 警告目标。

## 18. V8.8 激进现代化与入库性能优化记录

V8.8 新增事项：

- 已封存 V8.7 源码和可运行包：`E:\采集器\Modernized_V8.7_Milestone`、`E:\采集器\ModernizedOutput_V8.7_Milestone`。
- 真实过时 API 改代码消除：`Assembly.CodeBase`、`DESCryptoServiceProvider`、`WebRequest.Create`。
- 对 WinForms/反编译遗留字段和事件警告使用根级 `Directory.Build.props` 集中收敛，目标构建 `0` 警告。
- Jieqi 章节新增/按序新增热点写库改为单连接事务执行，关键 SQL 参数化。
- 继续保持 `jieqi_article_chapter.summary` 不写入、不更新、不依赖；不启用 5-10 分钟内存延迟写库。

## 19. V8.7 SQL Server 驱动现代化记录

V8.7 新增事项：

- 已封存 V8.6 源码和可运行包：`E:\采集器\Modernized_V8.6_Milestone`、`E:\采集器\ModernizedOutput_V8.6_Milestone`。
- 将启文适配器 `NovelSpider.Local.Qiwen` 从 `System.Data.SqlClient 4.9.1` 迁移到 `Microsoft.Data.SqlClient 6.1.1`。
- 主程序中调用启文 SQL Server 辅助方法的页面同步改用 `Microsoft.Data.SqlClient` 类型。
- 本次只迁移 SQL Server 客户端依赖，不改变 SQL 语句、存储过程调用、连接串配置入口和 UI 行为。

## 20. V8.6 低风险过时 API 收敛记录

V8.6 新增事项：

- 已封存 V8.5 源码和可运行包：`E:\采集器\Modernized_V8.5_Milestone`、`E:\采集器\ModernizedOutput_V8.5_Milestone`。
- 清理 `SecurityUtil` 中重复 `using`，减少编译噪音。
- 将 `SecurityUtil`、`CollectManual`、`NovelSpider.Local.Qiwen`、`NovelSpider.Local.Jieqi` 中部分旧 `TimeZone.CurrentTimeZone` 时间戳换算改为 `DateTimeOffset`。
- Jieqi 适配器新增内部 `FromUnixTimestamp` 轻量方法，集中处理 Unix 时间戳转本地时间。
- 本次不改变 UI、采集规则、数据库写入、旧 `HttpWebRequest` 和旧 DES 加密兼容行为。

## 21. V8.5 异常堆栈保真修复记录

V8.5 新增事项：

- 已封存 V8.4 源码和可运行包：`E:\采集器\Modernized_V8.4_Milestone`、`E:\采集器\ModernizedOutput_V8.4_Milestone`。
- 将 `ConfigFileManager`、`XmlConfig`、`Snapshot` 中的 `throw ex;` 改为 `throw;`，保留原始异常堆栈。
- 本次不改变配置读取、XML 处理和快照业务逻辑。

## 22. V8.4 Windows 平台警告收敛记录

V8.4 新增事项：

- 各程序集 `AssemblyInfo.cs` 增加 `[SupportedOSPlatform("windows")]`。
- 目标是减少 WinForms 和 System.Drawing 在 `.NET 8 Windows` 项目中的 CA1416 平台 API 警告。
- 本次不改变 UI、采集、写库或配置行为。

## 23. V8.3 章节摘要写入修复记录

V8.3 新增事项：

- 已封存 V8.2 源码和可运行包：E:\采集器\Modernized_V8.2_Milestone、E:\采集器\ModernizedOutput_V8.2_Milestone。
- jieqi_article_chapter 新增章节和分卷时不再写入 summary 字段。
- 新增/更新章节后只同步章节名，不再额外更新 jieqi_article_chapter.summary。
- UpdateLastChapter 不再读取章节表 summary，文章表 lastsummary 默认写空。

## 24. V8.2 TLS 网络请求修复记录

V8.2 新增事项：

- 修复 HTTPS 采集时报 The requested security protocol is not supported 的问题。
- 公共网络初始化统一使用 SecurityProtocolType.SystemDefault，避免强制启用已废弃的 Ssl3/Tls1.0/Tls1.1。
- NovelSpider.exe 启动时统一执行网络兼容初始化。
- 本次不改采集规则、Cookie、代理、编码和旧 HttpWebRequest 行为。

## 25. 当前 V8.0 基线记录

V8.0 已完成事项：

- 升级到 .NET 8 Windows WinForms。
- 主程序、配置库、管理台和高级工具文件版本统一为 `8.0.0.0`。
- 界面显示版本统一为 `8.0`。
- “最新消息”页面改为“更新日志”。
- DockPanelSuite 使用有效主题，修复主题未设置导致的崩溃。
- 修复 DockPanel 文档页标签栏被菜单遮挡的问题。
- 修复 `NovelAdmin.exe` 和 `NovelVip.exe` 缺失图标资源导致启动无响应的问题。
- 普通功能和高级功能默认可用。
- 已封存 V8.0 源码和可运行包。

## 26. 后续建议任务

可作为 V8.1 或后续版本计划：

- 逐步减少 WinForms 平台 API 警告。
- 清理旧授权判断残留，统一封装为本地功能状态。
- 持续维护独立更新日志资源 `src\NovelSpider\Resources\CHANGELOG.md`。
- 继续对照原版修正 UI 间距、标签栏和按钮显示。
- 检查 SQL 拼接逻辑，逐步改为参数化查询。
- 梳理采集规则导入导出和异常日志，提升排错能力。
- 为关键文本处理、拼音和分词逻辑补充最小单元测试。












- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。




