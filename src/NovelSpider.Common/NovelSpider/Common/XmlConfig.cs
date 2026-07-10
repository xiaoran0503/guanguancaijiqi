using System;
using System.Collections;
using System.Xml;

namespace NovelSpider.Common;

public class XmlConfig
{
	private Hashtable hashtable_0;

	public string this[string string_0]
	{
		get
		{
			if (hashtable_0.Contains(string_0))
			{
				return hashtable_0[string_0].ToString();
			}
			return string.Empty;
		}
		set
		{
			hashtable_0[string_0] = value;
		}
	}

	public static XmlConfig Settings => new XmlConfig("Config.xml");

	public XmlConfig(string string_0)
	{
		hashtable_0 = new Hashtable();
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.Load(string_0);
			foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
			{
				if (childNode.Name != "#comment")
				{
					hashtable_0.Add(childNode.Name, childNode.InnerText);
				}
			}
		}
		catch
		{
			throw;
		}
	}
}


