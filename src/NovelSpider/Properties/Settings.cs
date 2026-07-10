using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Properties;

[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
[CompilerGenerated]
internal sealed class Settings : ApplicationSettingsBase
{
	private static global::Properties.Settings defaultInstance;

	public static global::Properties.Settings Default => defaultInstance;

	static Settings()
	{
		defaultInstance = (global::Properties.Settings)SettingsBase.Synchronized(new global::Properties.Settings());
	}
}
