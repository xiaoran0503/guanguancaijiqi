using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Sunny.UI;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider;

/// <summary>
/// 组合根（Composition Root）：集中装配配置、日志与依赖注入容器。
/// 后续业务解耦时，服务在此注册、通过 AppServices.Provider 解析。
/// </summary>
internal static class AppServices
{
	public static IServiceProvider Provider { get; set; }
}

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		// ---- P0：配置 / 日志 / 依赖注入 组合根 ----
		IConfiguration configuration = null;
		try
		{
			configuration = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}
		catch (Exception ex)
		{
			// 日志初始化失败不应阻塞启动（静默日志器）
			Log.Logger = new LoggerConfiguration().CreateLogger();
			Console.Error.WriteLine("Serilog 初始化失败: " + ex.Message);
		}

		try
		{
			var services = new ServiceCollection();
			if (configuration != null)
			{
				services.AddSingleton(configuration);
			}
			services.AddLogging(builder => builder.AddSerilog());
			AppServices.Provider = services.BuildServiceProvider();
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("DI 容器构建失败: " + ex.Message);
		}

		// 授权验证已移除 - 全功能开放
		try
		{
			Net10RuntimeBootstrap.Initialize();
			if (!Directory.Exists("Log"))
			{
				Directory.CreateDirectory("Log");
			}
			Configs.LoadConfigs();
			Configs.BaseConfig.LicenseAd = "";
			Configs.BaseConfig.LicenseOk = true;
			Configs.BaseConfig.LicenseVip = true;
			Configs.BaseConfig.LicenseTime = DateTime.MaxValue;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "错误提示");
			return;
		}

		var loggerFactory = AppServices.Provider?.GetService<ILoggerFactory>();
		var logger = loggerFactory?.CreateLogger("NovelSpider.Startup");
		logger?.LogInformation("NovelSpider 启动，显示版本 {DisplayVersion}", Configs.DisplayVersion);

		try
		{
			NativeMethods.FreeConsole();

			// ---- P0：PerMonitorV2 高 DPI（多显示器独立缩放，清晰无模糊）----
			Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

			// ---- P1：.NET 10 原生跟随系统主题（浅色系统下即浅色外观）+ SunnyUI 全局蓝色样式 ----
			Application.SetColorMode(SystemColorMode.System);
			UIStyles.SetStyle(UIStyle.Blue);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new MdiForm());
		}
		finally
		{
			NativeMethods.FreeConsole();
			Log.CloseAndFlush();
		}
	}
}
