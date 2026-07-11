# NovelSpider Net10 Migration Notes

## Current Branch

- Source: `E:\采集器\Modernized_Net10_Working`
- Output: `E:\采集器\ModernizedOutput_Net10_Test`
- Baseline copied from: `E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- Target framework: `net10.0-windows`
- Platform target: `x64`
- Runtime identifier: `win-x64`
- SDK pinned by `global.json`: `10.0.301`
- Runtime tested against: `.NET 10.0.9`
- Test version: `10.4.3-net10-test / 10.4.3.0`

## Migration Boundaries

- Do not modify `E:\采集器\Modernized_Working` from this branch.
- Keep Qiwen archived only: no solution entry, no main-program reference, no published `NovelSpider.Local.Qiwen.dll` or `Microsoft.Data.SqlClient.dll`.
- Do not change XML rule formats or database schemas during the Net10 smoke phase.
- Use stable NuGet releases only for the active Net10 solution. Do not adopt beta/preview packages unless a future migration explicitly changes this policy.
- Keep archived `NovelSpider.Local.Qiwen` unchanged while archived. It may not restore against the active Net10 project graph because it remains a non-solution archival project.
- The main WinForms project embeds the legacy `.resources` files only so existing `GetString()` calls can load form text such as `NovelSpider.ConfigForm.resources`. Code no longer calls `GetObject()` for legacy form icons or toolbar images, avoiding BinaryFormatter deserialization in .NET 10.
- Program icons are native executable icons: `NovelSpider.exe` uses `Decompiled\NovelSpider\app.ico`; `NovelAdmin.exe` and `NovelVip.exe` use `Decompiled\NovelVip\app.ico`.
- `Net10RuntimeBootstrap` no longer sets `ServicePointManager`; .NET 10 uses runtime-native TLS defaults. The initializer now keeps only code-page registration and regex cache sizing.
- The active Net10 solution is Windows-only and x64-only: solution configurations use `x64`, active projects set `PlatformTarget=x64`, and publish uses `win-x64`.
- Startup UI avoids constructing the heavy `ConfigForm` during main-window load; the welcome page is deferred until after the main window is visible, and its changelog text is filled after the first paint.
- The hidden `WebBrowser` used only for image-to-text is now created when that feature is invoked, rather than while constructing `ConfigForm`.
- Dynamic XML rule regexes use a bounded cache with a 10-second timeout. Fixed internal patterns use `GeneratedRegex`; rule XML remains unchanged.
- Enabled performance telemetry batches CSV writes in the background and includes `ui` timing points for main-window load, welcome-page open, and configuration construction/open.
- GitHub Actions build and release automation is available on `net10-v10`, `main`, and `v10.*-net10` tags. CI uses repository-relative scripts and `runtime\Rules` / `runtime\Tasks` seed data.

## Dependency Audit

As of the Net10 dependency audit on 2026-07-09, the active solution has no stable NuGet updates available and no vulnerable packages reported by NuGet.

Active stable package baselines:

- `MySqlConnector 2.6.1`
- `Newtonsoft.Json 13.0.4`
- `System.Data.SQLite.Core 1.0.119`
- `SharpZipLib 1.4.2`
- `CHSPinYinConv 1.0.0`
- `jieba.NET 0.42.2`
- `DockPanelSuite 3.1.1`
- `DockPanelSuite.ThemeVS2015 3.1.1`
- `System.Management 10.0.9`
- `Microsoft.Extensions.DependencyInjection.Abstractions 10.0.9`
- `Microsoft.Extensions.Logging.Abstractions 10.0.9`

Preview/beta packages intentionally excluded:

- `Newtonsoft.Json 13.0.5-beta1`
- `Microsoft.Data.SqlClient 7.1.0-preview1.*`
- `.NET 11 preview` packages such as `System.Management 11.0.0-preview.*` and `Microsoft.Extensions.* 11.0.0-preview.*`

SQL Server dependency boundary:

- Active Net10 solution and publish output do not reference `System.Data.SqlClient` or `Microsoft.Data.SqlClient`.
- `NovelSpider.Local.Qiwen` remains archived with its historical `Microsoft.Data.SqlClient 6.1.1` reference and is excluded from active dependency modernization.
- If Qiwen is ever reactivated, migrate it separately to `net10.0-windows`, update `Microsoft.Data.SqlClient` to the latest stable version at that time, re-add solution/publish coverage, and validate SQL Server behavior.

## Local Machine Note

This machine has Windows `10.0.19045`. The .NET 10 supported OS metadata does not list normal Windows 10 22H2 as a clear supported desktop OS target. It is acceptable for local migration testing, but final release validation should be repeated on an officially supported Windows 11, Windows Server, or LTSC environment.

Reference: `https://builds.dotnet.microsoft.com/dotnet/release-metadata/10.0/supported-os.json`






- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。




