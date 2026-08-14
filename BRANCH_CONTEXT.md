# 当前工作区分支边界声明

本目录 `E:\缓存\shipsay\采集器\Modernized_Net10_Git_Working` 是 NovelSpider `.NET 10 / net10.0-windows` Git 测试分支。

- 迁移来源：`E:\缓存\shipsay\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- 当前有效版本：`10.18.5-net10-test / 10.18.5.0`
- 目标标签：`v10.18.5-net10`
- 当前用途：Net10 编译、发布、WinForms 资源、DockPanelSuite、采集/Jieqi 冒烟验证
- 禁止用途：回写 Net8 维护分支、恢复 Qiwen 运行入口、恢复 10.18.3 章节数据库缓存实验、引入 XML 规则格式或数据库 schema 迁移

Net8 仅保留最终基线源码和运行包：

```text
E:\缓存\shipsay\采集器\Modernized_Net8_Final_Baseline_V8.17.1
E:\缓存\shipsay\采集器\ModernizedOutput_Net8_Final_Baseline_V8.17.1
```

本分支默认发布目录：

```text
artifacts\NovelSpider-Net10-win-x64
```

本地测试输出目录：

```text
E:\缓存\shipsay\采集器\ModernizedOutput_Net10_Test
```

2026-08-08 起，采集器根目录只保留 Net8 最终基线和 Net10 当前维护版本；历史版本已归档到：

```text
E:\缓存\shipsay\temp\back\采集器_历史版本归档_20260808_101429
```

Qiwen 源码目录仅作为归档，不在解决方案、主程序引用或发布包中启用。

当前边界：

- XML 规则仍是唯一规则源；不得引入 JSON 规则副本或 DOM V11 规则格式。
- 当前 `V10.18.5` 维护线不包含 Jieqi 章节 SQLite 写库缓存实验相关实现。
- SQLite 日志模式修复保留，章节写库缓存不保留。
- 发布包继续排除 NovelAdmin、NovelVip、Qiwen 和 SqlClient 退役 DLL。
