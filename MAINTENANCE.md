# NovelSpider 维护清单

## 固定基线

- 当前主线：`net8.0-windows`
- 当前版本：`8.17.1`
- 当前工作目录：`E:\采集器\Modernized_Working`
- 当前发布目录：`E:\采集器\ModernizedOutput`
- Net8 最终基线源码：`E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- Net8 最终基线运行包：`E:\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1`
- 里程碑目录只读保存，不直接修改。
- 当前主线只支持 Jieqi；Qiwen 源码保留归档，但 UI、解决方案和发布包不再启用。
- 当前会话分支只做 Net8 维护；Net10 迁移必须另开会话和独立目录，禁止在 `Modernized_Working` 直接升级 `TargetFramework`。

## 常见修改入口

| 修改目标 | 优先查看 |
| --- | --- |
| 网络采集、TLS、代理、Cookie、连接池 | `src\NovelSpider.Common\NovelSpider\Common\HttpClient.cs`、`HttpTransportPool.cs` |
| 章节延时、规则解析、正文下载 | `src\NovelSpider.Target\NovelSpider\Target\Page.cs` |
| Jieqi 章节写库、TXT 生成 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\LocalProvider.cs` |
| MySQL 连接、事务、参数化 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\MySqlHelper.cs` |
| MySQL/MariaDB/Percona 兼容 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\DatabaseCompatibilityProfile.cs` |
| 原子文本写入 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\ChapterFileWriter.cs` |
| 封面图片边界 | `src\NovelSpider.Local.Jieqi\NovelSpider\Local\Jieqi\CoverImageService.cs` |
| CMS 兼容/旧配置迁移 | `src\NovelSpider.Config\NovelSpider\Config\CmsCompatibility.cs` |
| SQLite 采集日志 | `src\NovelSpider.Common\NovelSpider\Common\SpiderException.cs` |
| 更新日志 | `src\NovelSpider\Resources\CHANGELOG.md` |
| 版本号 | 各项目 `Properties\AssemblyInfo.cs` 和 `Configs.DisplayVersion` |

## 性能排查

- 性能 CSV 默认关闭；需要排障时，在启动程序前设置环境变量 `NOVELSPIDER_PERFORMANCE=1`。
- 启用后 CSV 输出目录：运行目录 `Log\Performance\yyyyMMdd.csv`。
- 重点看 `http`、`mysql`、`file`、`collect` 四类耗时。
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

## 开发约束

- 不改 XML 规则格式，除非明确做兼容迁移。
- 不默认启用纯内存延迟写库，避免崩溃丢章节。
- 新增数据库写入必须优先参数化。
- 新增文件写入优先走统一原子写入器。
- 不恢复 Qiwen UI 入口；若未来需要重新启用，必须另开分支并重新验证 SQL Server 依赖、发布包和配置迁移。
- 不在本分支做 Net10 迁移；若需要规划或实验，先从 Net8 最终基线复制 `Modernized_Net10_Working`。
- 大改前先封存当前可用源码和运行包。
