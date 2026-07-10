using System;
using System.Reflection;
using System.Windows.Forms;

namespace NovelSpider.Common;

public static class WinFormsNet10Features
{
	public static void ProtectSensitiveForm(Form form)
	{
		if (form == null)
		{
			return;
		}
		try
		{
			PropertyInfo property = typeof(Form).GetProperty("ScreenCaptureMode", BindingFlags.Instance | BindingFlags.Public);
			if (property?.PropertyType.IsEnum == true)
			{
				object value = Enum.Parse(property.PropertyType, "HideContent");
				property.SetValue(form, value);
			}
		}
		catch (Exception ex)
		{
			SpiderException.Debug("WinForms.ScreenCaptureMode", ex.Message);
		}
	}

	public static void SetClipboardText(string text)
	{
		Clipboard.SetText(text ?? string.Empty, TextDataFormat.UnicodeText);
	}
}