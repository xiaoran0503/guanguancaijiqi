# 关关采集器 / NovelSpider Net8 维护基线

本仓库保存的是 `NovelSpider` 的 `.NET 8 / net8.0-windows` 最终稳定基线，版本为 `V8.17.1 / 8.17.1.0`。

> 重要边界：本仓库只用于 Net8 维护分支。Net10 迁移、运行时升级、WinForms 资源大规模迁移等实验请在独立仓库、独立目录或独立分支中进行，不要混入本 Net8 基线。

## 当前定位

| 项目 | 内容 |
| --- | --- |
| 运行时 | `.NET 8` |
| TargetFramework | `net8.0-windows` |
| UI | Windows Forms + DockPanelSuite |
| 当前版本 | `8.17.1.0 / 8.17.1` |
| 维护 CMS | Jieqi |
| 已下线 CMS | Qiwen 已从 UI、主程序引用和发布包中隐藏/移除，源码仅作为归档 |
| 性能 CSV | 默认关闭；设置 `NOVELSPIDER_PERFORMANCE=1` 后临时启用 |
| 分支定位 | `net8-v8` 为 Net8/V8 最终维护基线分支 |

## 目录说明

| 路径 | 说明 |
| --- | --- |
| `src/` | 主程序、公共库、配置库、实体、Jieqi 本地适配等源码 |
| `libs/` | 项目保留的本地依赖 |
| `scripts/` | 构建、发布、版本检查、冒烟辅助脚本 |
| `resources_temp/` | 资源迁移和维护辅助文件 |
| `PROJECT_DEVELOPMENT.md` | 版本演进、技术栈、开发约束和发布流程 |
| `MAINTENANCE.md` | 后续维护入口说明 |
| `PERFORMANCE_BASELINE.md` | 性能基线和跟踪结论 |
| `NET8_FINAL_BASELINE_HANDOFF.md` | Net8 最终基线交接文档 |
| `BRANCH_CONTEXT.md` | 分支边界提醒 |

## 构建环境

开发机建议安装：

- Windows 10/11
- .NET 8 SDK
- Windows Desktop Runtime 8
- Git for Windows，当前维护机使用 `E:\PortableGit`

检查 SDK：

```powershell
dotnet --list-sdks
dotnet --list-runtimes
```

## 构建与验证

在仓库根目录执行：

```powershell
dotnet restore .\NovelSpider.sln
dotnet build .\NovelSpider.sln -c Release -v:minimal
dotnet list .\NovelSpider.sln package --vulnerable --include-transitive
```

发布和版本检查：

```powershell
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

GitHub Actions 只在 `net8-v8` 分支、面向 `net8-v8` 的 Pull Request、以及 `v*` tag 推送时自动执行同等构建流程。CI 使用相对路径脚本：

```powershell
.\scripts\publish-ci.ps1
```

自动发布结果：

- 每次成功运行都会上传 `NovelSpider-Net8-V8.17.1.zip` 构建产物
- 推送 `v*` tag 时会额外创建 GitHub Release 并附加 zip 包
- CI 不使用本机 `E:\采集器\ModernizedOutput`，避免污染本地发布目录

期望结果：

- Release 构建 `0` 错误、`0` 警告
- 漏洞包检查无已知漏洞
- `NovelSpider.exe`、`NovelAdmin.exe`、`NovelVip.exe` 文件版本均为 `8.17.1.0`

## 运行验证重点

每次维护后至少验证：

- `NovelSpider.exe`、`NovelAdmin.exe`、`NovelVip.exe` 能正常启动
- 配置页和自动采集页不闪退
- HTTP/HTTPS、GBK/GB2312、跳转、Cookie、代理路径可用
- 章节延时按毫秒配置生效，例如 `ChapterUrlWait=5000`
- Jieqi 新增章节、更新章节、最后章节刷新正常
- `jieqi_article_chapter.summary` 不写入、不更新
- TXT、目录页、全集、OPF、ZIP/JAR 输出与旧版本兼容

## 已完成的关键改造

- HTTPS/TLS 使用系统默认安全协议，避免旧 TLS 固定值导致失败
- GBK/GB2312 编码提供程序已注册，规则测试和采集入口可处理中文站点
- HTTP 采集保持 `HttpWebRequest` 兼容路径，优先稳定，不强制 HTTP/2
- Jieqi 高频写库路径已做事务复用和参数化改造
- 章节 TXT 与生成文件逐步收敛到原子写入器
- 性能 CSV 默认关闭，仅排障时通过环境变量启用
- Qiwen 功能从用户可见入口和发布包中下线
- Net8 最终基线已通过 Git 分支、tag 和外部 bundle 封存

## 维护边界

允许在本分支做：

- 采集规则兼容修复
- Jieqi/MySQL/MariaDB/Percona 兼容修复
- 低风险 SQL 参数化、事务复用和连接兼容修复
- TXT/HTML/OPF/ZIP/JAR 文件写入修补
- UI 空引用、配置缺省、运行包缺文件等稳定性修复
- 文档、脚本、发布流程和冒烟验证增强

不要在本分支做：

- 升级到 `net10.0-windows`
- 大规模迁移 WinForms `.resources`
- 大面积重命名反编译字段或重排窗体初始化逻辑
- 恢复 Qiwen 运行入口或 SQL Server 发布依赖
- 强制 HTTP/2、重写采集规则引擎或异步化整个采集管线
- 引入需要数据库迁移的新 CMS 类型或新规则 XML 格式

## Git 分支与里程碑

当前远程：

```text
https://github.com/xiaoran0503/guanguancaijiqi.git
```

推荐分支含义：

| 分支 / tag | 用途 |
| --- | --- |
| `net8-v8` | 独立 Net8/V8 最终维护基线分支，后续 Net8 维护只推送此分支 |
| `main` | 历史初始化分支，不再作为本会话 Net8 维护上传目标 |
| `v8.17.1-net8-final` | Net8 最终代码基线 tag |

本地外部里程碑 bundle：

```text
E:\采集器\Net8_Git_Milestones\guanguancaijiqi-v8.17.1-net8-final-981c45a.bundle
```

## Net10 迁移提醒

Net10 迁移请另开会话、另建目录、另建分支。建议从 Net8 最终基线复制起步，但不要把迁移实验回灌到本分支。

建议目录示例：

```text
E:\采集器\Modernized_Net10_Working
E:\采集器\ModernizedOutput_Net10_Test
```

迁移前应先处理 WinForms 二进制资源、`System.Drawing` 边界、DockPanelSuite 兼容性和旧设计器资源风险。
