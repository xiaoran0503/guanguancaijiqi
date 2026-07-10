# NovelSpider Net8 最终基线交接文档

## 1. 交接结论

当前 `Modernized_Working` 已作为 `.NET 8 / net8.0-windows` 维护分支继续保留，最终基线版本为 `V8.17.1`。

本分支后续只做 Net8 稳定维护、规则兼容修复、数据库兼容修复和低风险性能修补；不要在本分支做 `.NET 10` 迁移、WinForms 资源大改或运行时升级实验。

## 2. 最终基线封存目录

| 类型 | 路径 | 用途 |
| --- | --- | --- |
| Net8 最终基线源码 | `E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1` | `.NET 8` 最终可回退源码基线，只读保存 |
| Net8 最终基线运行包 | `E:\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1` | `.NET 8` 最终可运行包，只读保存 |
| Net8 维护工作区 | `E:\采集器\Modernized_Working` | 当前会话后续维护 Net8 分支使用 |
| Net8 维护输出 | `E:\采集器\ModernizedOutput` | 当前 Net8 分支发布输出，会被发布脚本覆盖 |
| 最近普通里程碑 | `E:\采集器\Modernized_V8.17.1_Milestone` | V8.17.1 普通里程碑 |
| 最近普通运行包 | `E:\采集器\ModernizedOutput_V8.17.1_Milestone` | V8.17.1 普通运行包 |

## 3. 当前技术状态

| 项 | 当前状态 |
| --- | --- |
| 运行时 | `.NET 8` |
| TargetFramework | `net8.0-windows` |
| UI | Windows Forms + DockPanelSuite |
| 主版本 | `8.17.1.0 / 8.17.1` |
| 主 CMS | Jieqi |
| Qiwen | 已从 UI、主程序引用、解决方案、发布包下线；源码仅归档 |
| 性能 CSV | 默认关闭；设置 `NOVELSPIDER_PERFORMANCE=1` 才启用 |
| Net10 | 不在本分支进行，另开会话/另开工作区规划 |

## 4. V8.17.1 已完成能力

- HTTPS/TLS 使用系统默认协议，保留 `HttpWebRequest` 兼容外层逻辑。
- HTTP 采集已回滚激进 HTTP/2 偏好，保持 HTTP/1.1 兼容优先。
- GBK/GB2312 编码页已注册，规则测试和采集入口不再报 `gbk is not a supported encoding name`。
- 章节延时按域名生效，`ChapterUrlWait=5000` 表示同域章节正文请求至少间隔约 5 秒。
- Jieqi 新增/更新章节继续不写入 `jieqi_article_chapter.summary`。
- Jieqi 新增章节、更新章节、最后章节刷新等路径已有事务和参数化基础。
- `InsertNovel` 已参数化并使用 `LAST_INSERT_ID()` 获取新书 ID。
- SQLite 采集日志热点已参数化。
- TXT/生成文件大量写入已收敛到 `ChapterFileWriter` 原子写入器。
- 封面保存已收敛到 `CoverImageService`，减少 `System.Drawing` 散落调用。
- `NovelSpider.Local.Qiwen.dll` 和 `Microsoft.Data.SqlClient.dll` 不再进入发布输出。

## 5. Net8 分支维护边界

允许在当前分支做：

- 采集规则兼容修复，例如站点改版、编码、跳转、Cookie、代理兼容。
- Jieqi/MySQL/MariaDB/Percona 兼容修复。
- 低风险 SQL 参数化、事务复用、连接串兼容修复。
- 低风险 TXT/HTML/OPF/ZIP/JAR 文件写入修补。
- UI 崩溃、空引用、配置缺省、运行包缺文件等稳定性修复。
- 文档、脚本、发布流程、冒烟验证增强。

不要在当前分支做：

- 升级 `TargetFramework` 到 `net10.0-windows`。
- 批量替换 WinForms `.resources` 或设计器资源。
- 大面积重命名反编译字段、重排 WinForms 控件初始化。
- 恢复 Qiwen 运行入口或 SQL Server 发布依赖。
- 强制 HTTP/2、异步化整个采集管线、重写规则引擎。
- 引入需要数据库迁移的新 CMS 类型或新规则 XML 格式。

## 6. Net10 新会话建议边界

新会话做 Net10 迁移时，建议不要直接修改当前 `Modernized_Working`。推荐先复制独立工作区，例如：

```text
E:\采集器\Modernized_Net10_Working
E:\采集器\ModernizedOutput_Net10_Test
```

Net10 分支应从 `E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1` 复制起步，而不是复用当前维护目录。这样 Net8 维护和 Net10 实验互不污染。

Net10 迁移前应先规划：

- WinForms `.resources` 二进制资源清理策略。
- `System.Drawing` 调用边界替换或隔离策略。
- DockPanelSuite 与 WinForms 设计器资源兼容性验证。
- 旧 BinaryFormatter 资源兼容风险。
- 包依赖升级矩阵和回滚策略。
- 三程序启动冒烟、规则测试、采集、MySQL 写入和 TXT 生成验收清单。

## 7. Net8 分支日常维护流程

1. 修改前先确认当前目录是 `E:\采集器\Modernized_Working`。
2. 不改 `TargetFramework`，不引入 `net10` 包或 `net10.0-windows` 目标。
3. 改动后执行：

```powershell
dotnet build "E:\采集器\Modernized_Working\NovelSpider.sln" -c Release -v:minimal
dotnet list "E:\采集器\Modernized_Working\NovelSpider.sln" package --vulnerable --include-transitive
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

4. 启动 `NovelSpider.exe`、`NovelAdmin.exe`、`NovelVip.exe` 做冒烟。
5. 若要采集性能诊断，临时设置：

```powershell
$env:NOVELSPIDER_PERFORMANCE = "1"
```

诊断后移除该变量，避免日常采集持续写 CSV。

## 8. 重要文件入口

| 目标 | 文件 |
| --- | --- |
| 网络请求 | `src\NovelSpider.Common\NovelSpider\Common\HttpClient.cs`、`HttpTransportPool.cs` |
| 编码/TLS/正则缓存初始化 | `src\NovelSpider.Common\NovelSpider\Common\NetworkCompatibility.cs` |
| 章节下载与延时 | `src\NovelSpider.Target\NovelSpider\Target\Page.cs` |
| Jieqi 写库和生成文件 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\LocalProvider.cs` |
| MySQL 辅助与事务 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\MySqlHelper.cs` |
| 原子文本写入 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\ChapterFileWriter.cs` |
| 封面图片边界 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\CoverImageService.cs` |
| CMS 兼容策略 | `src\NovelSpider.Config\NovelSpider\Config\CmsCompatibility.cs` |
| 性能跟踪开关 | `src\NovelSpider.Common\NovelSpider\Common\PerformanceTelemetry.cs` |
| 更新日志 | `src\NovelSpider\Resources\CHANGELOG.md` |
| 开发文档 | `PROJECT_DEVELOPMENT.md`、`MAINTENANCE.md`、`PERFORMANCE_BASELINE.md` |

## 9. 当前已知规则/站点状态

- `zgjpds.ith8.xyz`：已恢复采集兼容。
- `018-www.biquge85.com.xml`：已修复 `http/https` 规则匹配问题，列表可匹配。
- `2878150827048-www.biqubao10.com.xml`：目标站 Cloudflare 阻断，规则本身不是 GBK 问题；不建议在采集器内绕过 Cloudflare。

## 10. 当前性能结论

来自 `E:\采集器\20260709.csv` 的样本：

- 254 章采集总计约 `68.3s`，章节平均约 `269ms`。
- MySQL 总计约 `2.9s`，P95 约 `3ms`。
- TXT 写入总计约 `0.84s`，P95 约 `14ms`。
- 当前进一步提速优先看规则解析、模板处理和章节页生成；MySQL/TXT 单章写入不是主要瓶颈。

## 11. 交接提醒

当前会话后续建议只作为 Net8 维护分支使用。Net10 迁移请在新会话、新目录、新发布输出中规划和实施；不要把 Net10 改动带回 `Modernized_Working`，除非明确决定替换主线并重新封存新的 Net8 回退点。
