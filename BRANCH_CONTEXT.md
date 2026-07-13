# 当前工作区分支边界声明

本目录 `E:\采集器\Modernized_Net10_Working` 是 NovelSpider `.NET 10 / net10.0-windows` 迁移测试分支。

- 迁移来源：`E:\采集器\Modernized_Net8_Final_Baseline_V8.17.1`
- 当前测试版本：`10.18.2-net10-test / 10.18.2.0`
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






- V10.7.0: UI/后台等待现代化，移除自动采集 Run() 的 Application.DoEvents 忙等，自动采集/修复/配置图转文等待改为事件或可取消分段等待。
- V10.6.1: 修复 10.6.0 async 网络管线中超时 `OperationCanceledException` 直接冒泡的问题，恢复规则测试/采集请求超时返回空响应并按原重试语义处理。
- V10.6.0: 网络现代化大版本，将 HttpTransportPool/Common HttpClient 现代分支升级为 async/await 管线，Page 核心规则请求包装接入 async 节流、退避和同域并发租约；Jieqi 项目移除直接 SharpZipLib 依赖。
- V10.5.4: 稳态现代化第五阶段将普通 ZipLib 目录打包切换到 System.IO.Compression.ZipArchive，并为 HostRequestThrottle 增加 async 同域并发租约入口；UMD 特殊压缩继续保留 SharpZipLib。
- V10.5.3: 稳态现代化第四阶段保留 DockPanelSuite 并封装 Dock 打开入口，移除 active Jieqi 的 Newtonsoft.Json 依赖，封面保存收敛到公共 ImageService，网络解压热路径改用 System.IO.Compression，并为站点节流新增 async/cancellation 入口。
- V10.5.2: WinForms 现代化第三阶段清理 MessageForm / TaskForm 小窗体，使用空条件 Dispose、简化事件绑定和绘图类型名；大窗体继续保持保守改造。
- V10.5.1: WinForms 现代化第二阶段整理自动生成采集规则窗体，集中输入读取、忙碌状态、保存文件名规范化，并复用本地页面抓取 HttpClient。
- V10.5.0: WinForms 现代化第一阶段集中自动采集任务请求调度 UI 的加载、保存和规范化逻辑，清理任务保存/读取附近反编译式错误弹窗代码，保持界面行为不变。
- V10.4.0: 自动采集任务界面提供请求调度/站点友好访问配置，可直接设置随机延时区间、UA 模式、同域并发和失败退避，不需要手工修改 XML。




