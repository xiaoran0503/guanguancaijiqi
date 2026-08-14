# NovelSpider Net10

本仓库是 NovelSpider 的 `.NET 10 / Windows x64` 独立现代化分支，用于继续维护主采集器、WinForms 界面、Jieqi 入库、规则测试、自动采集和发布链路。

## 当前基线

- 当前版本：`10.18.5-net10-test / 10.18.5.0`
- 目标标签：`v10.18.5-net10`
- 当前分支：`net10-v10`
- 目标框架：`net10.0-windows`
- 运行平台：Windows-only `win-x64` / `x64`
- 固定 SDK：`global.json` 中的 `.NET SDK 10.0.301`
- 已测试运行时：`.NET 10.0.9`
- 源码基线：`Modernized_Net8_Final_Baseline_V8.17.1`
- 当前工作区：`E:\缓存\shipsay\采集器\Modernized_Net10_Git_Working`
- 默认发布目录：`artifacts\NovelSpider-Net10-win-x64`
- 本地测试输出：`E:\缓存\shipsay\采集器\ModernizedOutput_Net10_Test`

说明：`V10.18.5` 是当前 Net10 维护线。本版本继续不启用章节数据库缓存，不包含 `jieqi-chapter-buffer.db3` 相关代码。2026-08-08 起，采集器根目录只保留 Net8 最终基线和 Net10 当前维护版本，历史版本已归档到 `E:\缓存\shipsay\temp\back\采集器_历史版本归档_20260808_101429`。

## 当前范围

- 当前 active solution 只支持 Jieqi。
- `NovelSpider.Local.Qiwen` 仅作为归档源码保留，不进入解决方案、主程序引用、发布包或依赖现代化范围。
- `NovelAdmin`、`NovelVip` 属于归档项目，不构建、不发布、不作为 Net10 主线的一部分。
- 发布包不得包含 `NovelAdmin.exe`、`NovelVip.exe`、`NovelSpider.Local.Qiwen.dll`、`Microsoft.Data.SqlClient.dll`、`System.Data.SqlClient.dll`。
- NuGet 依赖只采用稳定版本，不使用 beta/preview 包。
- XML 采集规则仍是唯一规则源；本线不引入 DOM V11、JSON 规则副本或数据库 schema 迁移。

## 已完成的主要现代化

- 主程序迁移到 `.NET 10 Windows`，发布固定为 Windows x64。
- 网络与采集热路径逐步迁移到 `HttpClient`、`async/await`、`CancellationToken` 和同域请求调度。
- 自动采集主循环已从旧 `BackgroundWorker.DoWork` 主路径切到 async 执行链路，停止按钮可取消正在运行的采集任务。
- 规则测试、手工采集、修复/替换等高频路径持续拆除同步桥接和 UI 阻塞等待。
- Jieqi 写库热点逐步改为参数化 SQL、真实 async 包装和轻量 DTO/reader 读取。
- SQLite 日志模式修复：选择 SQLite 日志时不再额外生成文本 `Debug.Log`。
- DockPanelSuite 继续保留，新增 `DockWorkspaceService` 作为 Dock 打开入口封装；当前不引入 Krypton Toolkit。

## 构建与发布

在仓库根目录使用 PowerShell 执行：

```powershell
dotnet --info
.\scripts\bump-version.ps1 -ReleaseKind BugFix -ChangelogMessage "修复 xxx 问题。"
.\scripts\build-release.ps1
.\scripts\check-vulnerable.ps1
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

版本递增规则和自动改版本入口见 `RELEASE_PROCESS.md`。BUG 修复递增最后一位小版本号，新增功能递增中间版本号，Net 基线更新时头部主版本号等于 Net 基线版本号。

`publish-all.ps1` 默认发布到：

```text
artifacts\NovelSpider-Net10-win-x64
```

如需指定外部测试目录，可设置环境变量：

```powershell
$env:NOVELSPIDER_PUBLISH_DIR = "E:\缓存\shipsay\采集器\ModernizedOutput_Net10_Test"
.\scripts\publish-all.ps1
```

发布后必须确认：

- `NovelSpider.exe` 文件版本为当前版本。
- 发布包包含 `Rules`、`Tasks`、`Resources`。
- 发布包不包含 NovelAdmin、NovelVip、Qiwen 或 SqlClient 退役 DLL。

## GitHub Actions

`.github/workflows/net10-ci-release.yml` 会在以下场景运行：

- 推送到 `net10-v10`
- 推送到 `main`
- Pull Request 目标为 `net10-v10` 或 `main`
- 推送 `v10.*-net10` 标签
- 手动 `workflow_dispatch`

分支和 PR 构建会执行编译、依赖审计、发布、版本校验、压缩并上传 Windows x64 artifact。标签构建还会创建 GitHub Release 并上传 zip 包。

## 里程碑规则

常用 Net10 里程碑：

- `v10.0.1-net10`：首个 Windows x64 Net10 源码和运行包归档。
- `v10.1.0-net10`：DNS 进程缓存、大列表渐进加载、Jieqi 异步持久化桥接和性能遥测扩展。
- `v10.5.4-net10`：普通 Zip 路径切换到 `System.IO.Compression`，请求调度增加 async/cancellation 入口。
- `v10.6.1-net10`：修复 async 网络管线中超时取消异常直接冒泡的问题。
- `v10.7.0-net10`：UI/后台等待现代化，减少 `Application.DoEvents` 和忙等。
- `v10.18.1-net10`：修复自动采集启动后 UI 被主循环占用、停止按钮不可用的问题。
- `v10.18.2-net10`：修复 SQLite 日志模式仍写文本日志的问题，是上一版基线。
- `v10.18.4-net10`：修复原子写入编码参数失效、拼音化 SQL 和 `Read()` 误判问题，是上一版基线。
- `v10.18.5-net10`：依赖现代化收尾（PinYinConverterCore 1.0.2 / 内置 n-gram 分词 / Microsoft.Data.Sqlite 10.0.11），是当前有效基线。

每次新里程碑必须同步更新版本号、`src\NovelSpider\Resources\CHANGELOG.md`、README 和维护文档，并在构建、发布、版本检查通过后再打标签。

## 性能与排障

- 设置 `NOVELSPIDER_PERFORMANCE=1` 可记录 UI、采集、HTTP、MySQL、TXT/file 等性能数据。
- XML 规则正则使用缓存和 10 秒超时；规则异常应优先修正规则，不建议放宽超时。
- 请求调度是同 host 最小间隔，不是每个请求前固定 sleep。若页面解析或数据库处理已消耗间隔，界面可能看不到明显等待。
- 当前 `V10.18.5` 不启用章节 SQLite 写库缓存；章节入库仍按现有 Jieqi 写库语义执行。
- SQLite 日志和 SQLite 章节写库缓存是两回事：当前保留 SQLite 日志修复，不保留章节写库缓存重构。

## 相关文档

- `MAINTENANCE.md`：维护清单、发布检查和开发约束。
- `PROJECT_DEVELOPMENT.md`：项目结构、技术栈、构建发布和历史现代化记录。
- `NET10_MIGRATION_NOTES.md`：Net10 迁移边界、依赖审计和发布约束。
- `BRANCH_CONTEXT.md`：当前工作区和分支边界说明。
- `RELEASE_PROCESS.md`：版本递增规则、自动改版本脚本和发布检查流程。
- `DOCK_MIGRATION_ASSESSMENT.md`：DockPanelSuite / Krypton Toolkit 迁移评估。
- `PERFORMANCE_BASELINE.md`：性能基线和采集测试矩阵。
- `src\NovelSpider\Resources\CHANGELOG.md`：程序内更新日志来源。
