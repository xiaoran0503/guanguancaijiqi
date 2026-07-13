# NovelSpider Net10

This branch contains the independent .NET 10 / Windows x64 migration line for NovelSpider.

## Current Baseline

- Version: `10.11.0-net10-test / 10.11.0.0`
- `v10.3.1-net10`: Fixes the auto rule generator menu binding so it opens the rule generator instead of the retired update-novel form.
- `v10.3.2-net10`: Improves auto rule generation for real chapter directory pages such as `www.qbxs.net`, including reusable chapter URL patterns and clearer single-book rule notes.
- Branch: `net10-v10`
- Target framework: `net10.0-windows`
- Platform: Windows-only `win-x64` / `x64`
- SDK: `.NET SDK 10.0.301` pinned by `global.json`
- Runtime tested: `.NET 10.0.9`
- Source baseline: `Modernized_Net8_Final_Baseline_V8.17.1`
- Local test output: `E:\采集器\ModernizedOutput_Net10_Test`

## Scope

- Active solution supports Jieqi only.
- `NovelSpider.Local.Qiwen` remains archived and is not part of the active solution, publish output, or dependency modernization.
- The published package must not contain `NovelSpider.Local.Qiwen.dll`, `Microsoft.Data.SqlClient.dll`, or `System.Data.SqlClient.dll`.
- NuGet dependency policy is stable releases only; beta/preview packages are intentionally excluded.

## Build

Use PowerShell from the repository root:

```powershell
dotnet --info
.\scripts\build-release.ps1
.\scripts\check-vulnerable.ps1
.\scripts\publish-all.ps1
.\scripts\check-version.ps1
```

The active build scripts compile and publish as Windows x64. Build outputs are generated under `bin\x64\Release\net10.0-windows\win-x64`.

By default, `publish-all.ps1` writes to `artifacts\NovelSpider-Net10-win-x64`. Set `NOVELSPIDER_PUBLISH_DIR` to override the publish directory.

## GitHub Actions

`.github/workflows/net10-ci-release.yml` runs on:

- Pushes to `net10-v10`
- Pushes to `main`
- Pull requests targeting `net10-v10` or `main`
- Tags matching `v10.*-net10`
- Manual `workflow_dispatch`

Branch and pull request builds compile, audit, publish, verify, zip, and upload a Windows x64 artifact. Tag builds also create a GitHub Release and upload the zip package.

## Milestone

Net10 milestones:

- `v10.0.1-net10`: first Windows x64 Net10 source/runtime archive.
- `v10.0.2-net10`: GitHub Actions automatic build and release baseline.
- `v10.0.3-net10`: CI publish fallback path fix for GitHub Windows Runner.
- `v10.0.4-net10`: regex cache, lazy image browser, deferred welcome page, and buffered performance telemetry.
- `v10.1.0-net10`: DNS process cache, progressive large-list loading, async Jieqi persistence bridge, and expanded performance telemetry.
- `v10.1.1-net10`: database round-trip reduction, on-demand chapter text checks, and lighter Jieqi refresh queries.
- `v10.1.2-net10`: forced published changelog refresh, sensitive-form screen capture protection, and Unicode clipboard text path.
- `v10.3.1-net10`: fixes the auto rule generator menu binding so it opens the rule generator instead of the retired update-novel form.
- `v10.3.2-net10`: improves auto rule generation for real chapter directory pages such as `www.qbxs.net`, including reusable chapter URL patterns and clearer single-book rule notes.

Every future milestone should update the version, update `src\NovelSpider\Resources\CHANGELOG.md`, and create an independent Git tag.

## Performance

- XML rule regular expressions use a bounded cache and a 10-second execution timeout; internal fixed patterns use `GeneratedRegex`.
- The hidden `WebBrowser` used by image-to-text is created only when that feature is invoked.
- Set `NOVELSPIDER_PERFORMANCE=1` to record batched UI, collection, HTTP, MySQL, and file timing data.

## Notes

- See `NET10_MIGRATION_NOTES.md` for migration boundaries and dependency audit details.
- See `MAINTENANCE.md` for release checks and development constraints.
- See `PROJECT_DEVELOPMENT.md` for project structure and historical migration notes.

## Net10 V10.1.0 Performance Notes

- Direct HTTP requests use a 30-minute in-process DNS cache with a 512-host cap; proxy requests keep the default proxy path.
- Cached DNS entries are dropped when all cached addresses fail, then the same request resolves once again before surfacing the network error.
- Large WinForms lists append in 200-item UI batches so windows stay responsive during initial population.
- Set `NOVELSPIDER_PERFORMANCE=1` to record DNS, HTTP, UI batch, regex, TXT, and MySQL timing data under `Log\Performance`.





- V10.7.0: UI/后台等待现代化，移除自动采集 Run() 的 Application.DoEvents 忙等，自动采集/修复/配置图转文等待改为事件或可取消分段等待。
- V10.6.1: 修复 10.6.0 async 网络管线中超时 `OperationCanceledException` 直接冒泡的问题，恢复规则测试/采集请求超时返回空响应并按原重试语义处理。
- V10.6.0: 网络现代化大版本，将 HttpTransportPool/Common HttpClient 现代分支升级为 async/await 管线，Page 核心规则请求包装接入 async 节流、退避和同域并发租约；Jieqi 项目移除直接 SharpZipLib 依赖。
- V10.5.4: 稳态现代化第五阶段将普通 ZipLib 目录打包切换到 System.IO.Compression.ZipArchive，并为 HostRequestThrottle 增加 async 同域并发租约入口；UMD 特殊压缩继续保留 SharpZipLib。
- V10.5.3: 稳态现代化第四阶段保留 DockPanelSuite 并封装 Dock 打开入口，移除 active Jieqi 的 Newtonsoft.Json 依赖，封面保存收敛到公共 ImageService，网络解压热路径改用 System.IO.Compression，并为站点节流新增 async/cancellation 入口。
- V10.5.2: WinForms 现代化第三阶段清理 MessageForm / TaskForm 小窗体，使用空条件 Dispose、简化事件绑定和绘图类型名；大窗体继续保持保守改造。
- V10.5.1: WinForms 现代化第二阶段整理自动生成采集规则窗体，集中输入读取、忙碌状态、保存文件名规范化，并复用本地页面抓取 HttpClient。
- V10.5.0: WinForms 现代化第一阶段集中自动采集任务请求调度 UI 的加载、保存和规范化逻辑，清理任务保存/读取附近反编译式错误弹窗代码，保持界面行为不变。
- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。




