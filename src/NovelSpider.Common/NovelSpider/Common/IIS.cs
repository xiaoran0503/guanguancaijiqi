using System.DirectoryServices;

namespace NovelSpider.Common;

public class IIS
{
	public static string GetID(string string_0)
	{
		foreach (DirectoryEntry child in new DirectoryEntry("IIS://localhost/W3SVC").Children)
		{
			if (child.SchemaClassName == "IIsWebServer")
			{
				DirectoryEntry directoryEntry2 = new DirectoryEntry("IIS://localhost/W3SVC/" + child.Name + "/ROOT");
				if (string_0 == directoryEntry2.Properties["Path"].Value.ToString())
				{
					return child.Name;
				}
			}
		}
		return "";
	}

	public static string GetRealPath(string string_0, string string_1, string string_2)
	{
		foreach (DirectoryEntry child in new DirectoryEntry("IIS://localhost/W3SVC/1/ROOT/Down").Children)
		{
			bool flag = child.SchemaClassName == "IIsWebDirectory";
		}
		return "";
	}
}
