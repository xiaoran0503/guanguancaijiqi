using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NovelVip.Properties;

[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
[CompilerGenerated]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance;

	public static Settings Default => defaultInstance;

	[ApplicationScopedSetting]
	[DefaultSettingValue("http://www.baidu.com")]
	[SpecialSetting(SpecialSetting.WebServiceUrl)]
	[DebuggerNonUserCode]
	public string NovelVip_mxd_WebService => (string)this["NovelVip_mxd_WebService"];

	static Settings()
	{
		defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
