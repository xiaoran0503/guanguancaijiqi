# 当前工作区分支边界声明

本目录 `E:\采集器\Modernized_Working` 是 NovelSpider `.NET 8 / net8.0-windows` 维护分支。

- 当前最终基线：`V8.17.1 / 8.17.1.0`
- 当前用途：Net8 稳定维护、规则兼容、Jieqi/MySQL/TXT 低风险修复
- 禁止用途：Net10 迁移实验、运行时升级、WinForms 资源大迁移、强制 HTTP/2 重构

Net10 迁移请从以下只读基线复制新目录开始：

```text
E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1
```

建议新目录：

```text
E:\采集器\Modernized_Net10_Working
E:\采集器\ModernizedOutput_Net10_Test
```

如果后续必须在本目录实施非 Net8 维护类改动，先重新封存当前目录和输出目录，并在 `PROJECT_DEVELOPMENT.md` 顶部记录分支切换原因。
