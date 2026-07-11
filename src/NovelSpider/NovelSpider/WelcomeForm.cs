using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NovelSpider.Config;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class WelcomeForm : DockContent
{
	private IContainer icontainer_0;

	private Label label_0;

	private Label label_1;

	private Panel panel_0;

	private RichTextBox richTextBox1;

	private WebBrowser webBwr;

	public WelcomeForm()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.WelcomeForm));
		this.panel_0 = new System.Windows.Forms.Panel();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.webBwr = new System.Windows.Forms.WebBrowser();
		this.richTextBox1 = new System.Windows.Forms.RichTextBox();
		this.panel_0.SuspendLayout();
		base.SuspendLayout();
		this.panel_0.Controls.Add(this.label_0);
		this.panel_0.Controls.Add(this.label_1);
		this.panel_0.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel_0.Location = new System.Drawing.Point(0, 0);
		this.panel_0.Name = "panel_0";
		this.panel_0.Size = new System.Drawing.Size(584, 54);
		this.panel_0.TabIndex = 0;
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(12, 30);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(305, 12);
		this.label_0.TabIndex = 1;
		this.label_0.Text = "本软件只为替代用户重复手工劳动。严禁用于非法转载。";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(12, 9);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(281, 12);
		this.label_1.TabIndex = 0;
		this.label_1.Text = "请先获得小说作者授权及法律许可情况下转载小说。";
		this.webBwr.Dock = System.Windows.Forms.DockStyle.Fill;
		this.webBwr.Location = new System.Drawing.Point(0, 54);
		this.webBwr.MinimumSize = new System.Drawing.Size(20, 20);
		this.webBwr.Name = "webBwr";
		this.webBwr.Size = new System.Drawing.Size(584, 262);
		this.webBwr.TabIndex = 1;
		this.webBwr.Visible = false;
		this.richTextBox1.BackColor = System.Drawing.SystemColors.Control;
		this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.richTextBox1.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.richTextBox1.ForeColor = System.Drawing.Color.DarkRed;
		this.richTextBox1.Location = new System.Drawing.Point(0, 54);
		this.richTextBox1.Name = "richTextBox1";
		this.richTextBox1.ReadOnly = true;
		this.richTextBox1.Size = new System.Drawing.Size(584, 262);
		this.richTextBox1.TabIndex = 21;
		this.richTextBox1.Text = "";
		base.ClientSize = new System.Drawing.Size(584, 316);
		base.Controls.Add(this.richTextBox1);
		base.Controls.Add(this.webBwr);
		base.Controls.Add(this.panel_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "WelcomeForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "更新日志";
		base.Load += new System.EventHandler(WelcomeForm_Load);
		this.panel_0.ResumeLayout(false);
		this.panel_0.PerformLayout();
		base.ResumeLayout(false);
	}

	private void WelcomeForm_Load(object sender, EventArgs e)
	{
		Label label = label_0;
		string text = string.Concat(label.Text, " V", Configs.DisplayVersion, " Build ", Configs.Build.ToShortDateString());
		label.Text = text;
		richTextBox1.Text = "正在加载更新日志...";
		BeginInvoke(new MethodInvoker(LoadChangeLogText));
	}

	private void LoadChangeLogText()
	{
		if (IsDisposed)
		{
			return;
		}
		richTextBox1.Text = GetChangeLogText();
	}

	private static string GetChangeLogText()
	{
		string changeLogFile = Path.Combine(AppContext.BaseDirectory, "Resources", "CHANGELOG.md");
		if (File.Exists(changeLogFile))
		{
			return File.ReadAllText(changeLogFile);
		}
		return string.Join(Environment.NewLine, new string[]
		{
			"更新日志",
			"",
			"V10.4.4 Net10 Test    2026-07-11",
			"- 版本迭代为 10.4.4.0 / 10.4.4-net10-test。",
			"- 修复请求调度 UI 保存后不写入任务 XML，重新打开任务不回显、不生效的问题。",
			"- 随机延时区间、UA 模式、同域并发和失败退避现在保存任务后可稳定生效。",
			"",
			"V10.4.3 Net10 Test    2026-07-11",
			"- 版本迭代为 10.4.3.0 / 10.4.3-net10-test。",
			"- 修复 Jieqi 新增小说 INSERT SQL 字段名转义错误，解决 lastvolume 附近 SQL syntax error。",
			"",
			"V10.4.2 Net10 Test    2026-07-11",
			"- 版本迭代为 10.4.2.0 / 10.4.2-net10-test。",
			"- 请求调度面板移入采集进度页操作详情右侧空白区域，避免控件裁切。",
			"",
			"V10.4.1 Net10 Test    2026-07-11",
			"- 版本迭代为 10.4.1.0 / 10.4.1-net10-test。",
			"- 请求调度 UI 直接并入采集进度页右侧可见区域，打开即可看到随机延时、UA、并发和失败退避。",
			"",
			"V10.4.0 Net10 Test    2026-07-11",
			"- 版本迭代为 10.4.0.0 / 10.4.0-net10-test。",
			"- 自动采集任务 UI 新增请求调度配置区，支持随机延时区间、UA 模式、同域并发和失败退避。",
			"- 核心采集请求统一接入站点友好节流，降低请求突刺和误触限流概率。",
			"",
			"V10.3.5 Net10 Test    2026-07-11",
			"- 版本迭代为 10.3.5.0 / 10.3.5-net10-test。",
			"- 定向修复 www.biquge.club 自动生成规则，补齐 div#list/dl#newlist 目录、og:novel:author 作者和 /read 章节链接识别。",
			"",
			"V10.3.4 Net10 Test    2026-07-10",
			"- 版本迭代为 10.3.4.0 / 10.3.4-net10-test。",
			"- 自动生成规则补齐封面规则推断，qbxs 封面已验证可获取。",
			"",
			"V10.3.3 Net10 Test    2026-07-10",
			"- 版本迭代为 10.3.3.0 / 10.3.3-net10-test。",
			"- 自动生成规则支持从小说页反推 NovelKey 模板和首页最新列表规则。",
			"- qbxs 首页、小说页、目录页和正文链路已交叉验证。",
			"",
			"V10.3.2 Net10 Test    2026-07-10",
			"- 版本迭代为 10.3.2.0 / 10.3.2-net10-test。",
			"- 修复自动生成规则对真实目录页的章节链接和正文容器推断。",
			"- 自动规则预览提示单本规则与最新列表规则的边界。",
			"- 规则测试支持单本小说 ID 跳过最新列表校验。",
			"",
			"V10.3.1 Net10 Test    2026-07-10",
			"- 修复规则菜单自动生成采集规则误打开更新小说信息的问题。",
			"",
			"V10.3.0 Net10 Test    2026-07-10",
			"- 版本迭代为 10.3.0.0 / 10.3.0-net10-test。",
			"- 数据库写入强制 utf8mb4，章节 TXT 新写入固定 UTF-8 无 BOM。",
			"- 规则菜单新增自动生成采集规则，支持本地分析和 AI 建议入口。",
			"- 发布包只保留 NovelSpider.exe，NovelAdmin / NovelVip active 下架。",
			"- 帮助内容、查看日志、高级功能、MYSQL 时间换算菜单下架。",
			"",
			"V10.2.0 Net10 Test    2026-07-10",
			"- 版本迭代为 10.2.0.0 / 10.2.0-net10-test。",
			"- 技术兼容命名原生化，启动初始化改为 Net10RuntimeBootstrap。",
			"- CMS 与数据库配置检测改为 SupportedCms / DatabaseConnectionProfile。",
			"- Page 与手工采集热路径旧 ArrayList 改为泛型 List<T>。",
			"- async provider 调度改为 LocalProviderAsyncDispatcher，移除同步 GetResult 桥接。",
			"- Jieqi 分词改为 JiebaTextSegmenter 原生命名。",
			"- 移除 ScreenCaptureMode 反射兜底，当前 SDK 未暴露强类型 API，截图保护延后。",
			"",			"V10.1.2 Net10 Test    2026-07-10",
			"- 版本迭代为 10.1.2.0 / 10.1.2-net10-test。",
			"- 修复发布包 Resources\\CHANGELOG.md 未强制刷新导致运行包更新日志停留旧版的问题。",
			"- 发布脚本现在发布后强制复制源码更新日志到输出目录。",
			"- 配置页启用 .NET 10 WinForms 截图隐私保护；剪贴板文本写入改为 Unicode text API。",
			"",			"V10.1.1 Net10 Test    2026-07-10",
			"- 版本迭代为 10.1.1.0 / 10.1.1-net10-test。",
			"- 管理台章节列表改为按需检测正文，避免逐章读取造成 N+1 查询。",
			"- Jieqi 插章使用 LastInsertedId 获取自增 ID，减少数据库往返。",
			"- 最后章节刷新改用轻量 reader，异步路径补充真实 async 事务入口。",
			"",			"V10.1.0 Net10 Test    2026-07-10",
			"- 版本迭代为 10.1.0.0 / 10.1.0-net10-test。",
			"- 网络层新增 30 分钟进程内 DNS 缓存，代理链路保持默认行为。",
			"- 大列表改为每批 200 条渐进追加，降低 UI 首次填充卡顿。",
			"- Jieqi Provider 新增可选异步持久化接口，保持原事务和 SQL 语义。",
			"- 性能采样补齐 DNS、HTTP、UI 批次和 MySQL 阶段。",
			"",			"V10.0.4 Net10 Test    2026-07-10",
			"- XML 动态规则正则新增缓存、执行超时和 .NET 源生成固定正则，减少采集循环分配。",
			"- 配置页图片转文字浏览器改为按需创建；性能观测 CSV 改为后台批量异步写入。",
			"- 版本迭代为 10.0.4.0 / 10.0.4-net10-test。",
			"",
			"V10.0.3 Net10 Test    2026-07-10",
			"- 修复 GitHub Actions Windows Runner 没有本机 E:\\采集器 盘符时，发布脚本计算本机 fallback 目录导致失败的问题。",
			"- 发布脚本现在只在本机 fallback 根目录存在时才使用该路径；CI 默认使用仓库 runtime\\Rules / runtime\\Tasks 种子数据。",
			"- 版本迭代为 10.0.3.0 / 10.0.3-net10-test。",
			"",
			"V10.0.2 Net10 Test    2026-07-10",
			"- 新增 GitHub Actions 自动构建与自动发布流程：推送 net10-v10 / main 自动构建并上传 Windows x64 artifact，推送 v10.*-net10 tag 自动创建 GitHub Release。",
			"- 构建、漏洞检查、发布、版本检查脚本改为仓库相对路径，支持本机和 GitHub Runner 共用。",
			"- 新增 runtime\\Rules 和 runtime\\Tasks 种子数据，CI 发布包可自动补齐规则与任务目录。",
			"- 新增仓库 README.md 和 .gitignore，记录 Net10 Windows x64 分支、构建、发布和里程碑规则。",
			"- 版本迭代为 10.0.2.0 / 10.0.2-net10-test。",
			"",
			"V10.0.1 Net10 Test    2026-07-09",
			"- 优化主程序启动首屏响应：配置窗体改为首次打开配置菜单时懒加载，避免启动阶段同步构造大型 WinForms 页面。",
			"- 欢迎页更新日志改为窗体显示后再填充文本，减少首次 Dock 页打开时 RichTextBox 排版阻塞。",
			"- 延续 Windows-only win-x64 / x64 发布约束，版本迭代为 10.0.1.0 / 10.0.1-net10-test。",
			"",
			"V10.0.0 Net10 Test    2026-07-09",
			"- 从 Modernized_Net8_Final_Baseline_V8.17.1 复制独立工作区，迁移到 .NET 10 / net10.0-windows。",
			"- 固定 SDK 为 10.0.301，输出目录改为 ModernizedOutput_Net10_Test，文件版本改为 10.0.0.0。",
			"- 移除旧二进制 WinForms .resources 图标/工具栏图片资源，窗体改用原生系统应用图标。",
			"- 恢复窗体 .resources 字符串资源嵌入，修复配置窗体资源缺失异常；图标和工具栏图片仍不从旧资源反序列化。",
			"- 使用原始 app.ico 设置程序图标：NovelSpider 使用主程序图标，NovelAdmin 和 NovelVip 使用高级工具图标。",
			"- 清理旧 ServicePointManager 兼容设置，保留编码页注册和正则缓存初始化。",
			"- 将 Net10 相关 System.* / Windows 扩展包升级到 10.0.9，并将 Microsoft.Extensions.* 传递依赖提升到 10.0.9。",
			"- 完成 active Net10 solution 依赖审计：采用稳定最新版策略，当前无 stable outdated package、无 vulnerable package。",
			"- 修复 Net10 分析器警告，Release 构建达到 0 warning / 0 error。",
			"- Qiwen 继续作为归档源码保留，不进入解决方案、主程序引用或发布包。",
			"- 记录 Windows 10 10.0.19045 作为测试机的官方支持边界风险；正式发布前建议在受支持系统上复验。",
			"",
			"V8.17.1 Net8 Final Baseline    2026-07-09",
			"- 将 V8.17.1 / 8.17.1.0 确认为 .NET 8 / net8.0-windows 最终基线。",
			"- 新增 NET8_FINAL_BASELINE_HANDOFF.md 和 BRANCH_CONTEXT.md，明确当前工作区只用于 Net8 维护。",
			"- 封存最终基线源码与运行包：Modernized_Net8_Final_Baseline_V8.17.1、ModernizedOutput_Net8_Final_Baseline_V8.17.1。",
			"- Net10 迁移要求另开会话和独立工作区，建议从 Net8 最终基线复制起步。",
			"",
			"V8.17.1    2026-07-09",
			"- 默认关闭性能 CSV 跟踪，日常采集不再写入 Log\\Performance\\yyyyMMdd.csv。",
			"- 保留排障开关：启动前设置环境变量 NOVELSPIDER_PERFORMANCE=1、true、on 或 yes 时，才恢复性能 CSV 输出。",
			"- 根据 20260709.csv 分析，当前瓶颈主要在章节整体采集流程；MySQL 与 TXT 单章写入占比很低。",
			"- 继续保持 HTTP/1.1 兼容优先，不恢复强制 HTTP/2。",
			"",
			"V8.17    2026-07-09",
			"- 从 V8.13.3 稳定基线继续 net8 主线，封存 V8.13.3 PreV8.14 里程碑。",
			"- Qiwen 适配器停止作为可选 CMS：配置页只显示 Jieqi，旧 Qiwen 配置自动切回 Jieqi，主程序和解决方案不再引用 Qiwen 项目。",
			"- Jieqi 新增小说 InsertNovel 改为单连接事务内参数化插入，并使用 LAST_INSERT_ID() 获取自增书号。",
			"- 参数化章节读取、章节列表、章节页前后章查询、WAP 目录查询和 SQLite 日志写入热点。",
			"- 管理台保留手写 SQL，但对危险 SQL 增加二次确认；自动生成查询会先清理输入。",
			"- 扩展章节原子写入器为通用文本写入入口，部分目录/OPF/全集/JAR 文本写入走原子替换。",
			"- 封面保存收敛到 CoverImageService，为未来 net10 分支做准备。",
			"",
			"V8.13.3    2026-07-09",
			"- 进一步恢复 HTTP 下载路径到 V8.10.3 行为，移除网络层请求耗时插桩，优先保证最新列表和小说页规则兼容。",
			"- HttpTransportPool.Send() 重新保持“发送请求后直接返回响应”的旧行为，不在响应返回前写性能日志。",
			"- HttpClient.GetStringWork() / GetImageWork() 不再包裹 HTTP 性能统计，避免对老站点、反代站采集时序产生干扰。",
			"- 保留 GBK 编码页注册、Rules/Tasks 发布保护、MySQL/TXT 性能统计和章节按域名延时。",
			"",
			"V8.13.2    2026-07-09",
			"- 回滚 V8.13 中对采集请求强制优先 HTTP/2 的激进优化，恢复 V8.10.3 默认 HTTP/1.1 协商行为。",
			"- 保留连接池、TLS 复用、耗时统计和 GBK 编码页修复，但不再改变目标站协议偏好，避免列表页返回内容差异导致“没有获得小说列表”。",
			"- 移除 HTTP/2 keep-alive ping 相关设置，降低老小说站、反代站和规则正则的兼容风险。",
			"",
			"V8.13.1    2026-07-09",
			"- 修复规则测试和采集规则使用 gbk/gb2312 编码时，.NET 8 未注册代码页导致 'gbk' is not a supported encoding name 的问题。",
			"- 网络兼容初始化现在同时注册 CodePagesEncodingProvider，主程序和管理台会在加载配置前执行初始化。",
			"- FormatText.GetCharset() 增加编码注册兜底，GBK 配置明确返回 Encoding.GetEncoding(\"gbk\")。",
			"- Page 和公共 HttpClient 增加类内初始化兜底，避免规则测试入口绕过启动初始化。",
			"- 发布脚本会保留或补齐运行目录 Rules / Tasks，避免发版后缺少规则任务导致“当前小说页不存在”。",
			"",
			"V8.13    2026-07-09",
			"- 完成 net8 稳定基线下的性能、稳定性和可维护性阶段改造。",
			"- 增加性能观测日志，记录 HTTP、MySQL、TXT 文件写入和章节采集总耗时，输出到 Log\\Performance。",
			"- HTTPS 采集连接池增强 HTTP/2 自动协商、连接保活探测和请求耗时统计。",
			"- 章节正文请求改为按域名执行毫秒级节流，避免多 Page 实例绕过 ChapterUrlWait。",
			"- 章节 TXT 写入改为统一原子写入器，使用大缓冲和临时文件替换，降低半截文件风险。",
			"- MySQL 热点执行路径增加耗时统计，章节顺序调整和小说更新 SQL 改为参数化。",
			"- 更新日志迁移为独立 Markdown 资源，后续维护可直接修改 Resources\\CHANGELOG.md。",
			"",
			"V8.10.3    2026-07-09",
			"- 修复 HTTPS 连接池提速后，自动采集章节页延时可能被部分分支绕过的问题。",
			"- 章节正文请求入口统一执行 ChapterUrlWait 毫秒级节流，设置 5000 即至少间隔 5 秒请求下一章正文。",
			"",
			"V8.10.2    2026-07-09",
			"- 修复 MySQL 8/9 使用 caching_sha2_password 时，缺少 AllowPublicKeyRetrieval=True 导致连接失败的问题。",
			"- 数据库连接测试和保存配置时会在缺省情况下自动补齐 utf8mb4、SslMode=Preferred、AllowPublicKeyRetrieval=True。",
			"- 修复旧配置数值超过控件范围时，打开设置页 Value 越界崩溃的问题。",
			"",
			"V8.10.1    2026-07-09",
			"- 修复旧 BaseConfig.xml 缺少男女频/分类对应表等字段时，打开系统设置页面空引用崩溃的问题。",
			"- 配置加载后自动补齐缺失字符串和关键默认值，兼容旧配置文件继续使用。",
			"",
			"V8.10    2026-07-09",
			"- 从 V8.8 可信里程碑恢复 net8.0-windows 稳定基线，继续现代化和性能优化。",
			"- 保留自动采集窗口空 Rules/Tasks/Logs 目录和空配置路径的稳定性兜底，避免初始化闪退。",
			"- HTTPS 采集底层改为连接池传输层，复用连接与 TLS 握手，同时保持原同步采集、Cookie、代理和编码行为。",
			"- Jieqi 更新章节、刷新最后章节等热点写库路径改为单连接事务和参数化 SQL。",
			"- 统一章节 TXT 写入辅助，减少重复目录检查和散落文件写入代码。",
			"- 继续保持不写入 jieqi_article_chapter.summary，不启用延迟内存写库。",
			"",
			"V8.8    2026-07-09",
			"- 封存 V8.7 SQL Server 驱动现代化里程碑源码和运行包。",
			"- 修复 Assembly.CodeBase、DES、WebRequest.Create 等真实过时 API 警告。",
			"- 对 WinForms/反编译遗留字段警告采用集中规则收敛，目标构建零警告。",
			"- Jieqi 章节入库热点改为单连接事务写入并参数化关键 SQL，提升写入稳定性和性能。",
			"- 继续保持不写入 jieqi_article_chapter.summary，不启用延迟内存写库。",
			"",
			"V8.7    2026-07-09",
			"- 封存 V8.6 低风险过时 API 收敛里程碑源码和运行包。",
			"- 将启文适配器和主程序中使用的 System.Data.SqlClient 迁移到 Microsoft.Data.SqlClient。",
			"- 继续减少 .NET 8 下的过时依赖警告，保持原有数据库调用方式不变。",
			"- 本次不改变 UI、采集规则、MySQL/Jieqi 写库逻辑和 SQL Server 连接串配置入口。",
			"",
			"V8.6    2026-07-09",
			"- 封存 V8.5 异常堆栈保真修复里程碑源码和运行包。",
			"- 清理 SecurityUtil 中重复 using，减少编译噪音。",
			"- 将部分旧 TimeZone 时间戳换算改为 DateTimeOffset，继续降低 .NET 8 过时 API 警告。",
			"- 本次不改变 UI、采集规则、数据库写入和旧加密兼容行为。",
			"",
			"V8.5    2026-07-09",
			"- 封存 V8.4 Windows 平台警告收敛里程碑源码和运行包。",
			"- 修复 ConfigFileManager、XmlConfig、Snapshot 中 throw ex 导致异常堆栈丢失的问题。",
			"- 本次只修异常重抛方式，不改变配置读取、XML 处理和快照逻辑。",
			"",
			"V8.4    2026-07-09",
			"- 为各程序集增加 Windows 平台支持声明，逐步减少 WinForms/System.Drawing 平台 API 警告。",
			"- 本次只做平台标注和构建降噪，不改变采集、写库和界面行为。",
			"",
			"V8.3    2026-07-09",
			"- 封存 V8.2 TLS 网络请求修复里程碑源码和运行包。",
			"- 杰奇章节表 jieqi_article_chapter 不再写入 summary 字段，章节摘要直接丢弃。",
			"- 更新章节和刷新最后章节时不再依赖 jieqi_article_chapter.summary，避免额外写入和兼容问题。",
			"",
			"V8.2    2026-07-09",
			"- 修复 HTTPS 采集时报 The requested security protocol is not supported 的 TLS 协议兼容问题。",
			"- 移除旧版 Ssl3/Tls1.0/Tls1.1 强制组合，改用系统默认 TLS 策略。",
			"- 三个程序入口统一初始化网络兼容设置，保留原有采集规则、Cookie、代理和编码行为。",
			"",
			"V8.1    2026-07-09",
			"- 增强数据库兼容性，支持识别 MySQL、MariaDB、Percona Server。",
			"- MySQL 兼容范围明确为 5.7.x、8.x、9.x。",
			"- MySQL 8.x/9.x 自动补齐推荐连接参数：utf8mb4、SslMode=Preferred、AllowPublicKeyRetrieval=True。",
			"- MariaDB 自动使用 utf8mb4 与 Preferred SSL 兼容策略。",
			"- Percona Server 按 MySQL 协议兼容，5.7.x 保守处理，8.x 使用新版本策略。",
			"- 测试数据库时显示服务器类型、版本、字符集、排序规则和 SQL Mode。",
			"- 升级 MySqlConnector、Newtonsoft.Json、System.Data.SqlClient 等依赖。",
			"- 覆盖旧传递依赖，移除 System.Drawing.Common 4.7.0 高危漏洞链。",
			"",
			"V8.0    2026-07-09",
			"- 将软件版本号调整为 8.0，作为 .NET 8 现代化后的初始迭代版本。",
			"- 主程序、配置库、管理台和高级工具统一使用 8.0.0.0 程序集版本。",
			"- 原“最新消息”页面改为“更新日志”，后续版本变更统一在这里同步维护。",
			"- 升级到 .NET 8 Windows WinForms 运行环境，保留原版桌面软件操作习惯。",
			"- 修复 DockPanelSuite 在 .NET 8 下主题未初始化导致的打开页面报错。",
			"- 修复主界面文档标签栏位置异常，尽量贴近原版窗口排版。",
			"- 修复 NovelAdmin.exe 和 NovelVip.exe 启动时缺失图标资源导致无响应的问题。",
			"- 移除旧版 IP 授权限制，高级功能默认可直接使用。",
			"- 替换和整理旧 DLL 依赖，优先使用适配 .NET 8 的包和实现。",
			"- 更新发布输出目录为 E:\\采集器\\ModernizedOutput，便于直接运行。",
			"",
			"后续维护说明：",
			"- 每次功能调整、兼容性修复或依赖升级，都在本页面新增对应版本记录。",
			"- 版本号从 8.0 开始递增，例如 8.1、8.2 或 8.0.1。",
			"- 如发现界面和原版仍有差异，优先按原版布局继续修正。"
		});
	}
}















