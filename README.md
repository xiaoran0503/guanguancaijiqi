# NovelSpider Net10

This branch contains the independent .NET 10 / Windows x64 migration line for NovelSpider.

## Current Baseline

- Version: `10.0.3-net10-test / 10.0.3.0`
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

Every future milestone should update the version, update `src\NovelSpider\Resources\CHANGELOG.md`, and create an independent Git tag.

## Notes

- See `NET10_MIGRATION_NOTES.md` for migration boundaries and dependency audit details.
- See `MAINTENANCE.md` for release checks and development constraints.
- See `PROJECT_DEVELOPMENT.md` for project structure and historical migration notes.
