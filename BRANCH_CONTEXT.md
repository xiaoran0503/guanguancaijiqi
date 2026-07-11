# 当前工作区分支边界声明

本目录 `E:\采集器\Modernized_Net10_Working` 是 NovelSpider `.NET 10 / net10.0-windows` 迁移测试分支。

- 迁移来源：`E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- 当前测试版本：`10.4.2-net10-test / 10.4.2.0`
- 当前用途：Net10 编译、发布、WinForms 资源、DockPanelSuite、采集/Jieqi 冒烟验证
- 禁止用途：回写 Net8 维护分支、恢复 Qiwen 运行入口、引入 XML 规则格式或数据库 schema 迁移

Net8 维护仍在以下目录进行：

```text
E:\采集器\Modernized_Working
```

本分支输出目录：

```text
E:\采集器\ModernizedOutput_Net10_Test
```

Qiwen 源码目录仍仅作为归档，不在解决方案、主程序引用或发布包中启用。






- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。



