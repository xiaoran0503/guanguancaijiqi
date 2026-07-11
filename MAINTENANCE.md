# NovelSpider 维护清单

## 固定基线

- 当前主线：`net10.0-windows`
- 当前平台：Windows-only `win-x64` / `x64`
- 当前版本：`10.4.0-net10-test / 10.4.0.0`
- 当前工作目录：`E:\采集器\Modernized_Net10_Working`
- 当前发布目录：`E:\采集器\ModernizedOutput_Net10_Test`
- 固定 SDK：`.NET SDK 10.0.301`
- Net8 最终基线源码：`E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- Net8 最终基线运行包：`E:\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1`
- 里程碑目录只读保存，不直接修改。
- 当前主线只支持 Jieqi；Qiwen 源码保留归档，但 UI、解决方案和发布包不再启用。
- 本分支只做 Net10 迁移测试；不得把改动回写到 `Modernized_Working`，也不得恢复 Qiwen 运行入口。
- 依赖升级策略：当前 active Net10 solution 只采用 NuGet 稳定最新版，不使用 beta/preview 包；归档 Qiwen 项目不纳入依赖现代化。

## 常见修改入口

| 修改目标 | 优先查看 |
| --- | --- |
| 网络采集、TLS、代理、Cookie、连接池 | `src\NovelSpider.Common\NovelSpider\Common\HttpClient.cs`、`HttpTransportPool.cs` |
| 章节延时、规则解析、正文下载 | `src\NovelSpider.Target\NovelSpider\Target\Page.cs` |
| Jieqi 章节写库、TXT 生成 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\LocalProvider.cs` |
| MySQL 连接、事务、参数化 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\MySqlHelper.cs` |
| MySQL/MariaDB/Percona 连接配置 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\DatabaseConnectionProfile.cs` |
| 原子文本写入 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\ChapterFileWriter.cs` |
| 封面图片边界 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\CoverImageService.cs` |
| CMS Active CMS 配置 | `src\NovelSpider.Config\NovelSpider\Config\SupportedCms.cs` |
| SQLite 采集日志 | `src\NovelSpider.Common\NovelSpider\Common\SpiderException.cs` |
| 更新日志 | `src\NovelSpider\Resources\CHANGELOG.md` |
| 版本号 | 各项目 `Properties\AssemblyInfo.cs` 和 `Configs.DisplayVersion` |

## 性能排查

- 性能 CSV 默认关闭；需要排障时，在启动程序前设置环境变量 `NOVELSPIDER_PERFORMANCE=1`。
- 启用后 CSV 输出目录：运行目录 `Log\Performance\yyyyMMdd.csv`。
- 重点看 `ui`、`http`、`mysql`、`file`、`collect` 五类耗时。
- 性能日志采用后台批量异步写入，队列上限为 4096 条；它只用于排障，默认仍关闭。
- 动态 XML 规则正则经 `RuleRegexCache` 缓存，并有 10 秒执行超时；异常规则应修正规则，而不是放宽超时。
- 若采集过快，先确认 `TaskConfigInfo.ChapterUrlWait` 是否传入，并检查 `Page.GetChapterInfo()` 的域名限速。
- 若写库慢，先临时开启性能 CSV，看 MySQL 慢点集中在哪个 SQL 片段，再决定是否继续参数化或合并事务。
- 若 TXT 生成慢，先临时开启性能 CSV，并优先检查索引/全集生成是否在每章重复触发。
- 按 `20260709.csv` 当前样本，MySQL/TXT 已不是主要瓶颈；继续提速优先看规则解析、模板处理和章节页生成。
- 新增小说写入优先看 `InsertNovel` 的事务和参数化路径，避免回退到字符串拼接。
- 索引、全集、OPF、ZIP/JAR 属于生成型文件，优先任务级批量触发，不建议每章重复重建。

## 发布前检查

```powershell
.\scripts\build-release.ps1
.\scripts\check-vulnerable.ps1
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

GitHub Actions:

- Push `net10-v10` or `main` to run CI build and upload artifact.
- Push `v10.*-net10` tags to create GitHub Releases.
- Each milestone must update version metadata and `src\NovelSpider\Resources\CHANGELOG.md` before tagging.

## 开发约束

- 不改 XML 规则格式，除非明确做兼容迁移。
- 不默认启用纯内存延迟写库，避免崩溃丢章节。
- 新增数据库写入必须优先参数化。
- 新增文件写入优先走统一原子写入器。
- 不恢复 Qiwen UI 入口；若未来需要重新启用，必须另开分支并重新验证 SQL Server 依赖、发布包和配置迁移。
- 不把归档 `NovelSpider.Local.Qiwen` 的 `Microsoft.Data.SqlClient` 版本作为 active solution 依赖状态；Qiwen 重新启用时再独立迁移和验证 SQL Server。
- 不把本分支的 Net10/x64 改动回写到 `Modernized_Working` Net8 维护分支。
- 大改前先封存当前可用源码和运行包。

## V10.1.0 Runtime Notes

- DNS 缓存固定 30 分钟，仅驻留当前进程，不写入配置或磁盘。
- 代理请求不使用目标站 DNS 缓存，避免绕过代理链路。
- Jieqi 异步写库目前通过可选接口接入后台链路；其他 Provider 自动回退同步接口。
- 大列表加载以 200 条为一批追加，新增查询或重新加载会丢弃旧批次。

## V10.1.1 Database Notes

- 管理台章节列表默认显示 未检测，正文状态在详情读取时按需确认。
- Jieqi 插章使用 LastInsertedId 减少自增 ID 查询往返；失败仍回滚当前事务。
- 最后章节刷新使用 reader 读取单行，减少 DataTable 分配。


## V10.1.2 Changelog Notes

- publish-all.ps1 must force-copy src\\NovelSpider\\Resources\\CHANGELOG.md into output Resources\\CHANGELOG.md after publish.
- Release validation must inspect the published changelog top entry, not only the source changelog.


## V10.3.0 Native Notes

- 技术兼容层命名改为 Net10 原生命名；业务兼容仍保留 GBK/GB2312、XML 规则和 Jieqi 多版本字段策略。
- 当前 SDK 未暴露 WinForms ScreenCaptureMode 强类型 API，禁止使用反射兜底；该能力延后。
- Active async provider 调度统一在 `LocalProviderAsyncDispatcher`，不得新增 `GetAwaiter().GetResult()` 桥接。
- Jieqi 分词统一通过 `JiebaTextSegmenter`，不得恢复 `PanGu` 命名空间外壳。
- 原生化验收允许低频图像/UMD 老格式生成器继续保留 ArrayList/Hashtable。





## V10.3.0 Active Scope
- Active 发布包只保留 NovelSpider.exe；NovelAdmin / NovelVip 源码保留但不再进入 solution、CI 或发布包。
- 新写入 TXT 固定 UTF-8 无 BOM；CmsEncoding 仅用于网页解码和旧 TXT 读取。
- 数据库连接串保存和运行时统一强制 Charset=utf8mb4。
- 自动规则生成默认本地分析，AI Key 仅由用户在窗体中手动输入，不写入日志。





- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。

