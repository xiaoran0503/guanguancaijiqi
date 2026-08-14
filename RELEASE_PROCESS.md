# NovelSpider Net10 发版流程

本文档固定采集器 Net10 当前维护线的版本规则和发布动作。发版前默认只操作 `E:\缓存\shipsay\采集器\Modernized_Net10_Git_Working`，不修改 Net8 最终基线和历史归档目录。

## 版本规则

版本格式：

- 显示版本：`主版本.中间版本.小版本-net基线-test`
- 文件版本：`主版本.中间版本.小版本.0`
- 标签格式：`v主版本.中间版本.小版本-net基线`

递增规则：

- 修复 BUG 类：最后一位小版本号 `+1`。例如 `10.18.4` -> `10.18.5`。
- 新增功能类：中间版本号 `+1`，小版本归零。例如 `10.18.3` -> `10.19.0`。
- Net 基线更新：头部主版本号等于 Net 基线版本号，小版本归零。例如迁移到 Net11 时使用 `11.0.0-net11-test / 11.0.0.0`。

`NetBaseline` 只负责版本元数据和文档口径，不会自动修改 `TargetFramework`、SDK、NuGet 依赖或 CI runner。真正升级 .NET 基线时必须另行完成迁移验证。

## 自动改版本

发版前先运行版本脚本。脚本会更新 active Net10 项目的版本面：

- `src\NovelSpider.Config\NovelSpider\Config\Configs.cs`
- active 项目的 `Properties\AssemblyInfo.cs`
- `src\NovelSpider\Resources\CHANGELOG.md`
- `src\NovelSpider\NovelSpider\WelcomeForm.cs` 的内置兜底更新日志
- 当前维护文档中的当前版本和目标标签

BUG 修复发版：

```powershell
.\scripts\bump-version.ps1 -ReleaseKind BugFix -ChangelogMessage "修复 xxx 问题。"
```

新增功能发版：

```powershell
.\scripts\bump-version.ps1 -ReleaseKind Feature -ChangelogMessage "新增 xxx 功能。"
```

Net 基线发版：

```powershell
.\scripts\bump-version.ps1 -ReleaseKind NetBaseline -NetBaseline 11 -ChangelogMessage "迁移到 .NET 11 基线。"
```

需要指定精确版本时可以显式传入：

```powershell
.\scripts\bump-version.ps1 -ReleaseKind BugFix -Version 10.18.5 -ChangelogMessage "修复 xxx 问题。"
```

## 发布检查

版本脚本完成后执行：

```powershell
.\scripts\build-release.ps1
.\scripts\check-vulnerable.ps1
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

发布后必须确认：

- `NovelSpider.exe` 的 `ProductVersion` 和 `FileVersion` 等于本次文件版本。
- 发布包包含 `Rules`、`Tasks`、`Resources\CHANGELOG.md`。
- 发布包不包含 `NovelAdmin.exe`、`NovelVip.exe`、`NovelSpider.Local.Qiwen.dll`、`Microsoft.Data.SqlClient.dll`、`System.Data.SqlClient.dll`。
- 如果本机已有同名历史标签，不能直接移动标签；应先确认该标签是否属于已回滚实验或历史发布。

## 文档更新

脚本会处理采集器内部版本面。发版人仍需检查根目录总文档是否需要同步当前版本状态：

- `E:\缓存\shipsay\README.md`
- `E:\缓存\shipsay\技术说明.md`
- `E:\缓存\shipsay\开发文档.md`
- `E:\缓存\shipsay\交接文档.md`
- `E:\缓存\shipsay\项目说明.md`
- `E:\缓存\shipsay\项目关联.md`

若只是采集器内部实现修复且接口、数据流、部署规则不变，总文档只同步“当前版本”口径即可。
