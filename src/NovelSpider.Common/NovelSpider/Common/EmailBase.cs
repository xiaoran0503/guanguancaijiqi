namespace NovelSpider.Common;

public class EmailBase
{
	private string string_0;

	private string string_1;

	private string string_2;

	public string Content
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
		}
	}

	public string Subject
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
		}
	}

	public string ToEmail
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
		}
	}

	public EmailBase()
	{
	}

	public EmailBase(string toEmail, string subjectTemplate, string contentTemplate)
	{
		string_0 = toEmail;
		string_1 = subjectTemplate;
		string_2 = contentTemplate;
	}
}
