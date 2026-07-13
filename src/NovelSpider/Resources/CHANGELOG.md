V10.18.2 Net10 Test    2026-07-13
- 版本迭代为 `10.18.2.0 / 10.18.2-net10-test`。
- 修复选择 SQLite 日志模式后仍生成文本调试日志的问题：`SpiderException.Debug` 在 SQLite 模式下不再写入 `Debug.Log`。
- 修复无错误码任务日志绕过 SQLite 设置的问题：`SpiderException.Show(string, bool)` 与 `Show(string, NovelInfo, ...)` 在 SQLite 模式下写入 `TaskLog`。
- 文本日志模式保持原行为；SQLite 模式下错误码日志继续写入每日 `.db3`。

V10.18.1 Net10 Test    2026-07-13
- 版本迭代为 `10.18.1.0 / 10.18.1-net10-test`。
- 修复启动自动采集后 UI 被采集主循环占用的问题：开始按钮不再直接 await 主采集任务，采集主体改由 `RunAutoCollectOnBackgroundAsync` 在后台执行。
- 修复停止按钮不可用：停止按钮现在直接取消 `autoCollectCancellation`，并同步取消旧 `AutoWorker` 兼容状态。
- 自动采集进度与完成回调统一通过 `ReportAutoProgress` / `CompleteAutoCollect` 切回 UI 线程，避免后台采集直接更新 WinForms 控件。
- 关闭窗体时同时检查 `autoCollectRunning` 与旧 `AutoWorker.IsBusy`，防止采集仍在运行时直接关闭。

V10.18.0 Net10 Test    2026-07-13
- 版本迭代为 `10.18.0.0 / 10.18.0-net10-test`。
- `Page.Translate` 新增 `TranslateAsync`，翻译网络请求改为 `GetStringWorkAsync`；同步 `Translate` 仅作为旧路径兼容外壳保留。
- `Page` 新增 async `NovelInfo[] GetNovelListAsync`，列表解析中的小说名翻译可直接 await，减少同步网络阻塞。
- `CollectRepair` 的小说/目录阶段等待改为可取消分段等待，不再直接调用同步 `HostRequestThrottle.Wait`。
- 自动采集代理列表加载从 `LoginWorker.DoWork` 切到 `LoadProxyListAsync`，手动刷新和窗体加载均不再触发旧 `RunWorkerAsync`。
- 代码扫描中剩余 `GetStringWork/GetImageWork/HostRequestThrottle.Wait` 为显式同步兼容 API 或低频格式/兼容模块入口，活动采集/UI 路径优先使用 async。

V10.17.0 Net10 Test    2026-07-13
- 版本迭代为 `10.17.0.0 / 10.17.0-net10-test`。
- `Page` 新增 `GetIdsAsync`，最新列表 ID 抓取直接 await 现有 `GetStringWorkWithEmptyRetryAsync`，保留 DNS 缓存、同域并发、随机延时和失败退避。
- `Page` 静态其他站列表抓取新增 `GetNovelListAsync`，内部改用 `HttpClient.GetStringWorkAsync`；旧同步方法保留兼容外壳。
- 自动采集主循环的 `GetListUrl` 与 `OtherListUrl` 入口改为调用 async 列表抓取，减少列表阶段的同步阻塞。
- `CollectReplace` 等旧后台路径仍可通过同步兼容外壳调用，后续单独迁移。

V10.16.0 Net10 Test    2026-07-13
- 版本迭代为 `10.16.0.0 / 10.16.0-net10-test`。
- `CollectManual` 继续拆同步桥接：添加小说与替换章节入口改为 async helper，旧 `backgroundWorker_2_DoWork / backgroundWorker_8_DoWork` 归档，不再保留 `WaitForBackgroundAsync(.Wait/.Result)`。
- 手工替换章节正文抓取改用 `GetChapterInfoAsync`，最后章节刷新通过 `LocalProviderAsyncDispatcher.UpdateLastChapterAsync` 直接 await。
- 手工添加小说入库通过 `LocalProviderAsyncDispatcher.InsertNovelAsync` 直接 await，保持原进度回显和完成状态恢复。
- 自动采集辅助网络继续现代化：网络测速按钮改为 async `GetStringWorkAsync`，代理测速列表改为 async 请求并保持列表状态回写。
- 保留低频自动获取代理 IP 的 `LoginWorker` 同步分支，下一轮可单独替换并验证第三方代理页解析。

V10.15.0 Net10 Test    2026-07-13
- 版本迭代为 `10.15.0.0 / 10.15.0-net10-test`。
- 自动采集模式继续现代化：移除旧 `AutoWorker.DoWork` 事件绑定，`RunAutoCollectAsync / AutoWorkerCoreAsync` 成为唯一主采集执行入口，避免旧 BackgroundWorker 主循环被误触发。
- 自动采集内部等待从 `Thread.Sleep / WaitOrCancel` 改为 `Task.Delay + CancellationToken`，章节限速和异常短等待均可被停止操作取消。
- 站点友好延时在自动采集链路改为 `HostRequestThrottle.WaitAsync`，保留随机延时/UA/退避语义，同时减少后台线程阻塞。
- 默认更新 URL 拉取改为 `GetStringWorkAsync` 并接入取消令牌；命令行 `Run()` 保留兼容同步外壳，新增 `RunAsync` 供后续继续拆除同步入口。
- 下一轮继续拆手工采集剩余 `WaitForBackgroundAsync(LocalProviderAsyncDispatcher...)` 热点，以及测速/代理测试辅助 BackgroundWorker 的同步网络调用。

V10.14.0 Net10 Test    2026-07-13
- 版本迭代为 `10.14.0.0 / 10.14.0-net10-test`。
- `自动采集模式` 主循环主体拆为 `AutoWorkerCoreAsync`，`CollectNovel` / `CollectNovel_old` 改为 async 方法。
- 自动采集章节正文抓取改为 `await FetchChapterInfoForAutoAsync(...)`，新章节入库改为 `await InsertChapterForAutoAsync(...)`，减少采集线程中的同步桥接。
- 保留 `AutoWorker_DoWork` 兼容包装，下一步可继续清理旧 `BackgroundWorker` 事件绑定和剩余同步维护分支。

V10.13.0 Net10 Test    2026-07-13
- 版本迭代为 `10.13.0.0 / 10.13.0-net10-test`。
- `CollectManual` 旧 `backgroundWorker_6_DoWork` / `backgroundWorker_9_DoWork` 归档，Designer 不再绑定这两个旧 DoWork；追加章节、后续章节、选中章节和顺序插章入口统一走 async helper。
- `自动采集模式` 的按钮启动、定时启动和命令行 `Run()` 入口改为 `RunAutoCollectAsync` 包装，逐步脱离 `AutoWorker.RunWorkerAsync` 生命周期。
- 自动采集停止逻辑同步取消新 `CancellationTokenSource` 与旧 worker；取消判断统一到 `IsAutoCancellationPending()`，为下一步主循环内部章节抓取和入库全面 await 做准备。

V10.12.0 Net10 Test    2026-07-13
- 版本迭代为 `10.12.0.0 / 10.12.0-net10-test`。
- `CollectManual` 手工追加章节链路改为真正 async：章节正文使用 `Page.GetChapterInfoAsync`，章节入库使用 `LocalProviderAsyncDispatcher.InsertChapterAsync`，停止按钮可通过 `CancellationToken` 取消等待。
- `CollectManual` 顺序插章链路改为真正 async：正文抓取、顺序插章和最后章节刷新分别 await `GetChapterInfoAsync`、`InsertChapterByOrderAsync`、`UpdateLastChapterAsync`。
- `自动采集模式` 新增正文抓取、章节入库、最后章节刷新 async helper，为后续拆分 `AutoWorker` 大循环和移除同步桥接做准备。

V10.11.0 Net10 Test    2026-07-13
- 版本迭代为 `10.11.0.0 / 10.11.0-net10-test`。
- 网络层继续现代化：`HttpClient` 将 16k 特殊旧 socket 站点分支隔离为 legacy 方法，async 入口可通过后台执行路径复用旧站兼容逻辑，现代站点仍走 `HttpTransportPool` / `SocketsHttpHandler`。
- 采集层新增 `Page.GetChapterInfoAsync`，章节正文、二段正文 URL、分页正文请求复用 async retry、同域节流、失败退避、UA 策略和取消管线；旧同步 `GetChapterInfo` 保留兼容包装。
- 规则测试窗体接入 async 章节正文抓取，取消测试时正文页网络请求可以通过 `CancellationToken` 中断。
- Jieqi `MySqlHelper` 新增事务内 `ExecuteSingleRowAsync`，为后续小说、章节、分卷状态查询 DTO 化和真实 async 数据库链路继续铺路。

V10.10.0 Net10 Test    2026-07-13
- 版本迭代为 `10.10.0.0 / 10.10.0-net10-test`。
- V10.8.0 后台任务现代化：规则测试窗体已移除 `BackgroundWorker` 执行路径，改为 `async Task`、`CancellationTokenSource`、运行代次和分批日志追加；自动采集/手工采集大窗体保留分阶段拆解边界。
- V10.9.0 数据库读取现代化：`MySqlHelper` 新增事务内单行 reader 映射 helper，Jieqi 插入小说后的回读改为轻量 DTO，减少 `DataTable/DataRow` 分配。
- V10.10.0 类型质量：`Configs.TaskNovelInfo` 从 `Hashtable` 改为泛型字典，首批新封装模块启用 `#nullable enable` 并修正空值边界。
- 新增 `DOCK_MIGRATION_ASSESSMENT.md`，结论为 V10.x 保留 DockPanelSuite，通过 `DockWorkspaceService` 继续收敛依赖；Krypton Toolkit 仅作为独立 UI 大版本评估。

V10.7.0 Net10 Test    2026-07-13
- 版本迭代为 `10.7.0.0 / 10.7.0-net10-test`。
- 自动采集 `Run()` 移除 `Application.DoEvents` + `Thread.Sleep(1)` 忙等，改为 `RunWorkerCompleted` 事件等待。
- 自动采集中的短暂停顿、周期等待和主机检查等待改为可取消分段等待，停止任务时更快响应。
- `CollectRepair` 的规则延时等待改用统一 `HostRequestThrottle`，延时语义与任务请求调度保持一致。
- `ConfigForm` 图转文 WebBrowser 渲染等待改为 `ManualResetEventSlim` 事件等待，避免固定 100 秒轮询。

V10.6.1 Net10 Test    2026-07-13
- 版本迭代为 `10.6.1.0 / 10.6.1-net10-test`。
- 修复 V10.6.0 async 网络管线中请求超时 `OperationCanceledException` 直接冒泡的问题。
- 规则测试、列表抓取和封面下载在内部超时后恢复旧行为：记录调试信息并返回空响应/空图片，由上层按原有重试或“没有获得列表”语义处理。
- 保留外部显式 `CancellationToken` 的取消语义，后续窗体关闭或停止任务仍可真正取消请求。

V10.6.0 Net10 Test    2026-07-13
- 版本迭代为 `10.6.0.0 / 10.6.0-net10-test`。
- `HttpTransportPool` 新增真实 `SendAsync`，统一记录 async 网络耗时、取消、超时和网络错误。
- `NovelSpider.Common.HttpClient` 现代站点分支新增 `GetStringWorkAsync` / `GetImageWorkAsync`，同步入口桥接到 async 管线，保留 16k 特殊 socket 分支。
- `Page` 核心规则请求包装接入 async 节流、失败退避和同域并发租约，减少采集线程阻塞等待。
- Jieqi active 项目移除直接 SharpZipLib 包引用；Common 仅为 UMD 特殊压缩继续保留 SharpZipLib。

V10.5.4 Net10 Test    2026-07-13
- 版本迭代为 `10.5.4.0 / 10.5.4-net10-test`。
- 将普通 `ZipLib` 目录打包实现迁移到 `System.IO.Compression.ZipArchive`，减少 SharpZipLib 在常规 zip 场景的使用。
- 为 `HostRequestThrottle` 增加 async 同域并发租约入口，后续采集链路可使用 `WaitAsync` / `EnterAsync` 逐步替代阻塞等待。
- UMD 生成器中的特殊 Deflater 压缩暂时保留 SharpZipLib，避免扩大低频格式风险。

V10.5.3 Net10 Test    2026-07-13
- 版本迭代为 `10.5.3.0 / 10.5.3-net10-test`。
- 保留 DockPanelSuite，新增 DockWorkspaceService 统一主窗口 DockContent 打开入口，降低后续 UI 容器替换风险。
- 将 active Jieqi 的百度推送 JSON 解析迁移到 `System.Text.Json.Nodes`，移除 Newtonsoft.Json 包引用。
- 新增公共 `ImageService` 隔离 System.Drawing 封面保存逻辑，为后续 ImageSharp/SkiaSharp 替换预留边界。
- 将网络 gzip/deflate 解压热路径改为 `System.IO.Compression`，减少对 SharpZipLib 的普通 HTTP 解压依赖。
- 为 HostRequestThrottle 新增 async/cancellation 等待入口，并移除规则测试中的硬编码短暂停顿。

V10.5.2 Net10 Test    2026-07-11
- 版本迭代为 `10.5.2.0 / 10.5.2-net10-test`。
- WinForms 现代化第三阶段：清理 `MessageForm` 与 `TaskForm` 小窗体，使用空条件 Dispose、简化事件绑定和绘图类型名。
- 保持大窗体保守改造策略；本轮未继续拆分 `自动采集模式.cs`，避免中文控件名和历史字符串处理引入风险。
V10.5.1 Net10 Test    2026-07-11
- 版本迭代为 `10.5.1.0 / 10.5.1-net10-test`。
- WinForms 现代化第二阶段：整理 `RuleAutoGenerateForm`，将规则生成输入读取集中到 `RuleGenerationInput`，减少按钮事件直接读取控件的分散逻辑。
- 自动生成规则窗体新增统一忙碌状态方法和保存文件名规范化 helper，避免保存规则时重复散落清理逻辑。
- 本地页面抓取改为复用静态 `System.Net.Http.HttpClient`，保持 30 秒超时和现有 UA 语义，减少重复创建连接对象。
V10.5.0 Net10 Test    2026-07-11
- 版本迭代为 `10.5.0.0 / 10.5.0-net10-test`。
- 启动 WinForms 现代化第一阶段：集中自动采集任务“请求调度/站点友好访问”的加载、保存和规范化逻辑，后续新增调度字段只需维护单一映射点。
- 清理自动采集模式中任务保存/读取附近的反编译式错误弹窗临时变量，统一走 `ShowErrorMessage`，降低 UI 事件代码噪音。
- 轻量现代化欢迎页版本标题拼接，保持界面与资源加载行为不变。
V10.4.4 Net10 Test    2026-07-11
- 版本迭代为 `10.4.4.0 / 10.4.4-net10-test`。
- 修复自动采集任务界面“请求调度/站点友好访问”设置保存后不写入任务 XML 的问题。
- 修复重新打开任务时随机延时区间、UA 模式、同域并发和失败退避不回显、不生效的问题；旧固定延时继续同步为对应区间最小值。
V10.4.3 Net10 Test    2026-07-11
- 版本迭代为 `10.4.3.0 / 10.4.3-net10-test`。
- 修复 Jieqi 入库 INSERT SQL 字段名转义错误，`typeid` / `totalcost` 不再被 C# 字符串转义破坏为制表符残缺字段。
- 解决新增小说时 MySQL 在 `lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords` 附近报 SQL syntax error 的问题。
V10.4.2 Net10 Test    2026-07-11
- 版本迭代为 `10.4.2.0 / 10.4.2-net10-test`。
- 按实际运行界面调整请求调度 UI，将面板移入“操作详情”右侧空白区域，避免右上角空间不足导致控件裁切。
- 请求调度面板完整显示四类请求最小/最大随机延时、失败退避、同域并发和 UA 模式。
V10.4.1 Net10 Test    2026-07-11
- 版本迭代为 `10.4.1.0 / 10.4.1-net10-test`。
- 修复 V10.4.0 请求调度 UI 放置不可靠的问题，将配置直接并入“采集进度”页右侧原“延时等待”可见区域。
- 右侧区域现改名为“请求调度”，直接显示列表/信息/目录/正文的最小/最大随机延时、失败退避、同域并发和 UA 模式。
V10.4.0 Net10 Test    2026-07-11
- 版本迭代为 `10.4.0.0 / 10.4.0-net10-test`。
- 自动采集任务 UI 新增“请求调度 / 站点友好访问”配置区，可设置列表、信息、目录、正文四类请求的随机延时区间。
- 新增任务级 UA 模式：固定全局 UA、随机 PC 浏览器 UA、随机手机浏览器 UA、随机爬虫 UA；随机模式按 host 稳定选择内置 UA。
- 核心采集请求统一接入同域随机节流、同域并发上限和 403/429/503/空响应失败退避，降低请求突刺和误触限流概率。
- 保留旧任务固定延时兼容，旧 `NovelUrlWait` / `IndexUrlWait` / `ChapterUrlWait` 会在 UI 中回显为对应区间。
V10.3.5 Net10 Test    2026-07-11
- 版本迭代为 `10.3.5.0 / 10.3.5-net10-test`。
- 回滚未完成的多站点兼容实验到 V10.3.4 基线，定向修复 `www.biquge.club` 自动生成规则。
- 自动规则生成器新增 `div#list` / `dl#newlist` / 常见章节列表 class 目录容器识别，支持 `/read/{NovelKey}/{ChapterKey}.html` 章节链接。
- 首页小说列表优先匹配带文本的 `/book/{NovelKey}/` 链接，避免图片封面链接污染小说名。
- 小说作者新增 `og:novel:author` 识别，封面、目录、正文通过核心 Page API 验证。
V10.3.4 Net10 Test    2026-07-10

- 版本迭代为 `10.3.4.0 / 10.3.4-net10-test`。
- 自动规则生成器补齐封面规则推断，优先识别 `og:image`、小说页主图 `.pic img` 和常见 image/cover 路径。
- 修正 `www.qbxs.net` 规则种子的 `NovelCover`，封面可从 `https://www.qbxs.net/images/0/1/1s.jpg` 正常获取。
- 用核心采集器验证 `神话从童子功开始` 封面获取成功，图片尺寸 `300x400`。
V10.3.3 Net10 Test    2026-07-10

- 版本迭代为 `10.3.3.0 / 10.3.3-net10-test`。
- 自动规则生成器会从示例小说页反推 `NovelUrl` / `PubIndexUrl` 的 `{NovelKey}` 模板，例如 `https://www.qbxs.net/book/1/` 推为 `https://www.qbxs.net/book/{NovelKey}/`。
- 自动规则生成器会抓取站点首页并从同类 `/book/数字/` 链接推断 `NovelListUrl` / `NovelList_GetNovelKey`，避免单本规则在最新列表测试阶段误报失败。
- 用 `www.qbxs.net` 首页、小说页、目录页和章节页交叉验证：最新列表、小说信息、章节目录和正文链路均可命中。
V10.3.2 Net10 Test    2026-07-10

- 版本迭代为 `10.3.2.0 / 10.3.2-net10-test`。
- 修复自动生成规则对 `www.qbxs.net` 这类真实目录页识别不足的问题，补齐章节目录区域、章节链接和正文容器推断。
- 生成的章节正文地址改为可复用 `{ChapterKey}` 路径，不再把示例章节页 URL 写死到所有章节。
- 自动规则预览增加单本规则提示：未提供站点最新列表页时不会自动生成 `NovelListUrl`，需优先测试小说信息、目录和正文。
- 规则测试窗体支持填写单本小说 ID 时跳过最新列表测试，避免单本规则被旧的列表校验误报失败。
V10.3.1 Net10 Test    2026-07-10

- 版本迭代为 `10.3.1.0 / 10.3.1-net10-test`。
- 修复规则菜单“自动生成采集规则”误绑定到旧“更新小说信息”入口的问题。
- 自动规则生成入口改为独立菜单项和事件处理，点击后打开 `RuleAutoGenerateForm`。
V10.3.0 Net10 Test    2026-07-10

- 版本迭代为 `10.3.0.0 / 10.3.0-net10-test`。
- 数据库连接串强制归一化为 `Charset=utf8mb4`；新写入章节 TXT 固定 UTF-8 无 BOM，旧 GBK/GB2312 TXT 继续兼容读取。
- 规则菜单新增“自动生成采集规则”，支持本地页面分析生成 RuleConfigInfo XML 草稿，并提供 OpenAI-compatible AI 建议入口。
- 发布包瘦身为主程序 `NovelSpider.exe`，`NovelAdmin` / `NovelVip` 从 active solution、CI 和发布包下架，源码保留归档。
- 帮助菜单下架帮助内容、查看日志、高级功能；辅助菜单下架 MYSQL 时间换算。
- 新增采集正文质量评分和失败分类 helper，用于规则生成和后续性能诊断。
V10.2.0 Net10 Test    2026-07-10

- 版本迭代为 `10.2.0.0 / 10.2.0-net10-test`。
- 技术兼容命名原生化：`NetworkCompatibility` 改为 `Net10RuntimeBootstrap`，`CmsCompatibility` 改为 `SupportedCms`，数据库配置检测改为 `DatabaseConnectionProfile`。
- 移除 V10.1.2 的 WinForms `ScreenCaptureMode` 反射兜底；当前 SDK 未暴露强类型 API，截图保护延后。
- `Page` 与手工采集热路径的旧 `ArrayList` 改为泛型 `List<T>`，减少运行时转换和装箱风险。
- `LocalProviderAsyncBridge` 改为 `LocalProviderAsyncDispatcher`，移除同步 `GetAwaiter().GetResult()` 桥接写法。
- Jieqi 分词移除旧 `PanGu` 命名空间外壳，改为 `JiebaTextSegmenter` 原生命名。
- RuleForm 规则说明表和 MySql 参数缓存改为泛型集合 / ConcurrentDictionary。
- WinForms 剪贴板辅助改为 `WinFormsRuntime.SetClipboardText`，继续使用 Unicode text API。
V10.1.2 Net10 Test    2026-07-10

- 版本迭代为 `10.1.2.0 / 10.1.2-net10-test`。
- 修复发布包 `Resources\CHANGELOG.md` 未强制刷新，导致运行包更新日志停留在旧版本的问题。
- 发布脚本现在在发布完成后强制复制源码更新日志到输出目录，避免多项目发布覆盖顺序影响。
- 配置页启用 .NET 10 WinForms 截图隐私保护；剪贴板文本写入改为 Unicode text API。
V10.1.1 Net10 Test    2026-07-10

- 版本迭代为 `10.1.1.0 / 10.1.1-net10-test`。
- 管理台章节列表改为按需检测正文，避免加载列表时逐章读取正文造成 N+1 查询。
- Jieqi 插章路径使用 MySqlConnector LastInsertedId 获取自增 ID，减少插入后 `SELECT LAST_INSERT_ID()` 往返。
- 最后章节刷新改用轻量 reader，避免 DataTable 分配；异步路径补充真实 async 事务入口。
V10.1.0 Net10 Test    2026-07-10

- 版本迭代为 `10.1.0.0 / 10.1.0-net10-test`。
- 网络层新增 30 分钟进程内 DNS 缓存，仅直连请求启用；代理链路保持系统默认解析与代理转发。
- DNS 缓存最多保留 512 个主机条目，缓存 IP 全部连接失败时立即失效并在当前请求重新解析一次。
- 管理台小说/章节列表和代理列表改为每批 200 条渐进追加，降低首次打开和大列表填充时的 UI 卡顿。
- Jieqi Provider 新增可选异步持久化接口，自动/手工采集后台链路优先走异步桥接并保持原事务和 SQL 语义。
- 采集热路径补齐 HTTP、空响应重试、DNS、UI 批次和 MySQL 阶段性能采样；`NOVELSPIDER_PERFORMANCE=1` 时写入性能 CSV。
更新日志

V10.0.4 Net10 Test    2026-07-10
- XML 动态规则正则新增容量上限缓存与 10 秒执行超时，目录、章节、搜索和替换链路避免重复创建 Regex；固定内部格式改用 .NET 源生成正则。
- 配置页的图片转文字 WebBrowser 改为按需创建，首次打开系统设置不再初始化隐藏浏览器控件；欢迎页继续在主窗体显示后再创建。
- 性能 CSV 观测改为后台批量异步写入；新增主窗体、欢迎页、配置页构造/打开计时点，默认仍关闭。
- 版本迭代为 10.0.4.0 / 10.0.4-net10-test。

V10.0.3 Net10 Test    2026-07-10
- 修复 GitHub Actions Windows Runner 没有本机 `E:\采集器` 盘符时，发布脚本计算本机 fallback 目录导致失败的问题。
- 发布脚本现在只在本机 fallback 根目录存在时才使用该路径；CI 默认使用仓库 `runtime\Rules` / `runtime\Tasks` 种子数据。
- 版本迭代为 `10.0.3.0 / 10.0.3-net10-test`。

V10.0.2 Net10 Test    2026-07-10
- 新增 GitHub Actions 自动构建与自动发布流程：推送 `net10-v10` / `main` 自动构建并上传 Windows x64 artifact，推送 `v10.*-net10` tag 自动创建 GitHub Release。
- 构建、漏洞检查、发布、版本检查脚本改为仓库相对路径，支持本机和 GitHub Runner 共用。
- 新增 `runtime\Rules` 和 `runtime\Tasks` 种子数据，CI 发布包可自动补齐规则与任务目录。
- 新增仓库 `README.md` 和 `.gitignore`，记录 Net10 Windows x64 分支、构建、发布和里程碑规则。
- 版本迭代为 `10.0.2.0 / 10.0.2-net10-test`。

V10.0.1 Net10 Test    2026-07-09
- 优化主程序启动首屏响应：配置窗体改为首次打开配置菜单时懒加载，避免启动阶段同步构造大型 WinForms 页面。
- 欢迎页更新日志改为窗体显示后再填充文本，减少首次 Dock 页打开时 RichTextBox 排版阻塞。
- 延续 Windows-only `win-x64` / `x64` 发布约束，版本迭代为 `10.0.1.0 / 10.0.1-net10-test`。

V10.0.0 Net10 Test    2026-07-09
- 从 `Modernized_Net8_Final_Baseline_V8.17.1` 复制独立工作区，迁移到 `.NET 10 / net10.0-windows`。
- 固定 SDK 为 `10.0.301`，输出目录改为 `ModernizedOutput_Net10_Test`，文件版本改为 `10.0.0.0`。
- 移除旧二进制 WinForms `.resources` 图标/工具栏图片资源，窗体改用原生系统应用图标，避免 .NET 10 BinaryFormatter 资源反序列化问题。
- 恢复窗体 `.resources` 字符串资源嵌入，修复打开配置窗体时 `NovelSpider.ConfigForm.resources` 缺失导致的 JIT 异常；图标和工具栏图片仍不从旧资源反序列化。
- 使用原始 `app.ico` 设置程序图标：`NovelSpider.exe` 使用 NovelSpider 图标，`NovelAdmin.exe` 和 `NovelVip.exe` 使用 NovelVip 图标。
- 将 Net10 active solution 和发布链路收紧为 Windows-only `win-x64` / `x64`，不再保留 Any CPU 编译配置。
- 清理旧 `ServicePointManager` 兼容设置，保留编码页注册和正则缓存初始化。
- 将 Net10 相关 `System.*` / Windows 扩展包升级到 `10.0.9`，并将 `Microsoft.Extensions.*` 传递依赖提升到 `10.0.9`。
- 完成 active Net10 solution 依赖审计：采用稳定最新版策略，当前无 stable outdated package、无 vulnerable package；beta/preview 包和归档 Qiwen 依赖不纳入本轮升级。
- 修复 Net10 分析器警告，Release 构建达到 0 warning / 0 error。
- Qiwen 继续作为归档源码保留，不进入解决方案、主程序引用或发布包。
- 记录 Windows 10 10.0.19045 作为测试机的官方支持边界风险；正式发布前建议在受支持系统上复验。

V8.17.1 Net8 Final Baseline    2026-07-09
- 将 `V8.17.1 / 8.17.1.0` 确认为 `.NET 8 / net8.0-windows` 最终基线。
- 新增 `NET8_FINAL_BASELINE_HANDOFF.md` 和 `BRANCH_CONTEXT.md`，明确当前工作区只用于 Net8 维护。
- 封存最终基线源码与运行包：`Modernized_Net8_Final_Baseline_V8.17.1`、`ModernizedOutput_Net8_Final_Baseline_V8.17.1`。
- Net10 迁移要求另开会话和独立工作区，建议从 Net8 最终基线复制起步，避免污染当前维护分支。

V8.17.1    2026-07-09
- 默认关闭性能 CSV 跟踪，日常采集不再写入 Log\Performance\yyyyMMdd.csv，减少热路径文件追加开销。
- 保留排障开关：启动前设置环境变量 NOVELSPIDER_PERFORMANCE=1、true、on 或 yes 时，才恢复性能 CSV 输出。
- 根据 20260709.csv 分析，当前瓶颈主要在章节整体采集流程；MySQL 与 TXT 单章写入占比很低，后续优化优先看规则解析、模板处理和章节页生成。
- 继续保持 HTTP/1.1 兼容优先，不恢复强制 HTTP/2。

V8.17    2026-07-09
- 从 V8.13.3 稳定基线继续 net8 主线，封存 V8.13.3 PreV8.14 里程碑。
- Qiwen 适配器停止作为可选 CMS：配置页只显示 Jieqi，旧 Qiwen 配置自动切回 Jieqi，主程序和解决方案不再引用 Qiwen 项目。
- Jieqi 新增小说 `InsertNovel` 改为单连接事务内参数化插入，并使用 `LAST_INSERT_ID()` 获取自增书号，减少文本拼接和重复连接。
- 参数化章节读取、章节列表、章节页前后章查询、WAP 目录查询和 SQLite 日志写入热点。
- 管理台保留手写 SQL，但对危险 SQL 增加二次确认；自动生成的 ID 列表和关键词查询会先清理输入。
- 扩展章节原子写入器为通用文本写入入口，全集 TXT、JAR 分片、MANIFEST 和部分目录/OPF/全集 HTML 走原子替换。
- 封面保存收敛到 `CoverImageService`，减少 `System.Drawing` 在业务逻辑中的散落调用，为未来 net10 分支做准备。

V8.13.3    2026-07-09
- 进一步恢复 HTTP 下载路径到 V8.10.3 行为，移除网络层请求耗时插桩，优先保证最新列表和小说页规则兼容。
- `HttpTransportPool.Send()` 重新保持“发送请求后直接返回响应”的旧行为，不在响应返回前写性能日志。
- `HttpClient.GetStringWork()` / `GetImageWork()` 不再包裹 HTTP 性能统计，避免对老站点、反代站采集时序产生干扰。
- 保留 GBK 编码页注册、Rules/Tasks 发布保护、MySQL/TXT 性能统计和章节按域名延时。

V8.13.2    2026-07-09
- 回滚 V8.13 中对采集请求强制优先 HTTP/2 的激进优化，恢复 V8.10.3 默认 HTTP/1.1 协商行为。
- 保留连接池、TLS 复用、耗时统计和 GBK 编码页修复，但不再改变目标站协议偏好，避免列表页返回内容差异导致“没有获得小说列表”。
- 移除 HTTP/2 keep-alive ping 相关设置，降低老小说站、反代站和规则正则的兼容风险。

V8.13.1    2026-07-09
- 修复规则测试和采集规则使用 gbk/gb2312 编码时，.NET 8 未注册代码页导致 `'gbk' is not a supported encoding name` 的问题。
- 网络兼容初始化现在同时注册 `CodePagesEncodingProvider`，主程序和管理台会在加载配置前执行初始化。
- `FormatText.GetCharset()` 增加编码注册兜底，GBK 配置明确返回 `Encoding.GetEncoding("gbk")`，避免被系统默认编码带偏。
- `Page` 和公共 `HttpClient` 增加类内初始化兜底，避免规则测试入口绕过启动初始化。
- 发布脚本会保留或补齐运行目录 `Rules` / `Tasks`，避免发版后缺少规则任务导致“当前小说页不存在”。

V8.13    2026-07-09
- 完成 net8 稳定基线下的性能、稳定性和可维护性阶段改造。
- 增加性能观测日志，记录 HTTP、MySQL、TXT 文件写入和章节采集总耗时，输出到 Log\Performance。
- HTTPS 采集连接池增强 HTTP/2 自动协商、连接保活探测和请求耗时统计。
- 章节正文请求改为按域名执行毫秒级节流，避免多 Page 实例绕过 ChapterUrlWait。
- 章节 TXT 写入改为统一原子写入器，使用大缓冲和临时文件替换，降低半截文件风险。
- MySQL 热点执行路径增加耗时统计，章节顺序调整和小说更新 SQL 改为参数化。
- 更新日志迁移为独立 Markdown 资源，后续维护可直接修改 Resources\CHANGELOG.md。
- 新增维护文档、性能基准文档和 scripts 维护脚本入口，方便后续开发和发布。

V8.10.3    2026-07-09
- 修复 HTTPS 连接池提速后，自动采集章节页延时可能被部分分支绕过的问题。
- 章节正文请求入口统一执行 ChapterUrlWait 毫秒级节流，设置 5000 即至少间隔 5 秒请求下一章正文。

V8.10.2    2026-07-09
- 修复 MySQL 8/9 使用 caching_sha2_password 时，缺少 AllowPublicKeyRetrieval=True 导致连接失败的问题。
- 数据库连接测试和保存配置时会在缺省情况下自动补齐 utf8mb4、SslMode=Preferred、AllowPublicKeyRetrieval=True。
- 修复旧配置数值超过控件范围时，打开设置页 Value 越界崩溃的问题。

V8.10.1    2026-07-09
- 修复旧 BaseConfig.xml 缺少男女频/分类对应表等字段时，打开系统设置页面空引用崩溃的问题。
- 配置加载后自动补齐缺失字符串和关键默认值，兼容旧配置文件继续使用。

V8.10    2026-07-09
- 从 V8.8 可信里程碑恢复 net8.0-windows 稳定基线，继续现代化和性能优化。
- 保留自动采集窗口空 Rules/Tasks/Logs 目录和空配置路径的稳定性兜底，避免初始化闪退。
- HTTPS 采集底层改为连接池传输层，复用连接与 TLS 握手，同时保持原同步采集、Cookie、代理和编码行为。
- Jieqi 更新章节、刷新最后章节等热点写库路径改为单连接事务和参数化 SQL。
- 统一章节 TXT 写入辅助，减少重复目录检查和散落文件写入代码。
- 继续保持不写入 jieqi_article_chapter.summary，不启用延迟内存写库。

V8.8    2026-07-09
- 封存 V8.7 SQL Server 驱动现代化里程碑源码和运行包。
- 修复 Assembly.CodeBase、DES、WebRequest.Create 等真实过时 API 警告。
- 对 WinForms/反编译遗留字段警告采用集中规则收敛，目标构建零警告。
- Jieqi 章节入库热点改为单连接事务写入并参数化关键 SQL，提升写入稳定性和性能。
- 继续保持不写入 jieqi_article_chapter.summary，不启用延迟内存写库。

V8.7    2026-07-09
- 封存 V8.6 低风险过时 API 收敛里程碑源码和运行包。
- 将启文适配器和主程序中使用的 System.Data.SqlClient 迁移到 Microsoft.Data.SqlClient。
- 继续减少 .NET 8 下的过时依赖警告，保持原有数据库调用方式不变。
- 本次不改变 UI、采集规则、MySQL/Jieqi 写库逻辑和 SQL Server 连接串配置入口。

V8.6    2026-07-09
- 封存 V8.5 异常堆栈保真修复里程碑源码和运行包。
- 清理 SecurityUtil 中重复 using，减少编译噪音。
- 将部分旧 TimeZone 时间戳换算改为 DateTimeOffset，继续降低 .NET 8 过时 API 警告。
- 本次不改变 UI、采集规则、数据库写入和旧加密兼容行为。

V8.5    2026-07-09
- 封存 V8.4 Windows 平台警告收敛里程碑源码和运行包。
- 修复 ConfigFileManager、XmlConfig、Snapshot 中 throw ex 导致异常堆栈丢失的问题。
- 本次只修异常重抛方式，不改变配置读取、XML 处理和快照逻辑。

V8.4    2026-07-09
- 为各程序集增加 Windows 平台支持声明，逐步减少 WinForms/System.Drawing 平台 API 警告。
- 本次只做平台标注和构建降噪，不改变采集、写库和界面行为。

V8.3    2026-07-09
- 封存 V8.2 TLS 网络请求修复里程碑源码和运行包。
- 杰奇章节表 jieqi_article_chapter 不再写入 summary 字段，章节摘要直接丢弃。
- 更新章节和刷新最后章节时不再依赖 jieqi_article_chapter.summary，避免额外写入和兼容问题。

V8.2    2026-07-09
- 修复 HTTPS 采集时报 The requested security protocol is not supported 的 TLS 协议兼容问题。
- 移除旧版 Ssl3/Tls1.0/Tls1.1 强制组合，改用系统默认 TLS 策略。
- 三个程序入口统一初始化网络兼容设置，保留原有采集规则、Cookie、代理和编码行为。

V8.1    2026-07-09
- 增强数据库兼容性，支持识别 MySQL、MariaDB、Percona Server。
- MySQL 兼容范围明确为 5.7.x、8.x、9.x。
- MySQL 8.x/9.x 自动补齐推荐连接参数：utf8mb4、SslMode=Preferred、AllowPublicKeyRetrieval=True。
- MariaDB 自动使用 utf8mb4 与 Preferred SSL 兼容策略。
- Percona Server 按 MySQL 协议兼容，5.7.x 保守处理，8.x 使用新版本策略。
- 测试数据库时显示服务器类型、版本、字符集、排序规则和 SQL Mode。
- 升级 MySqlConnector、Newtonsoft.Json、System.Data.SqlClient 等依赖。
- 覆盖旧传递依赖，移除 System.Drawing.Common 4.7.0 高危漏洞链。

V8.0    2026-07-09
- 将软件版本号调整为 8.0，作为 .NET 8 现代化后的初始迭代版本。
- 主程序、配置库、管理台和高级工具统一使用 8.0.0.0 程序集版本。
- 原“最新消息”页面改为“更新日志”，后续版本变更统一在这里同步维护。
- 升级到 .NET 8 Windows WinForms 运行环境，保留原版桌面软件操作习惯。
- 修复 DockPanelSuite 在 .NET 8 下主题未初始化导致的打开页面报错。
- 修复主界面文档标签栏位置异常，尽量贴近原版窗口排版。
- 修复 NovelAdmin.exe 和 NovelVip.exe 启动时缺失图标资源导致无响应的问题。
- 移除旧版 IP 授权限制，高级功能默认可直接使用。
- 替换和整理旧 DLL 依赖，优先使用适配 .NET 8 的包和实现。
- 更新发布输出目录为 E:\采集器\ModernizedOutput，便于直接运行。

后续维护说明：
- 每次功能调整、兼容性修复或依赖升级，都在本页面新增对应版本记录。
- 版本号从 8.0 开始递增，例如 8.1、8.2 或 8.0.1。
- 如发现界面和原版仍有差异，优先按原版布局继续修正。


















