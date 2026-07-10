using System;

namespace NovelSpider.Entity;

public class ChapterInfo
{
	private DateTime _lastTime;

	private DateTime _postTime;

	private int _itemIndex;

	private int _putId;

	private int _size;

	public string Name;

	private string _volumeName;

	private string _chapterName;

	private string _chapterText;

	private string _getId;

	private Uri _chapterUrl;

	private Uri _textUrl;

	public string Summary;

	public string ChapterName
	{
		get
		{
			return _chapterName;
		}
		set
		{
			_chapterName = value;
		}
	}

	public string ChapterText
	{
		get
		{
			return _chapterText;
		}
		set
		{
			_chapterText = value;
		}
	}

	public int chaptertype { get; set; }

	public Uri ChapterUrl
	{
		get
		{
			return _chapterUrl;
		}
		set
		{
			_chapterUrl = value;
		}
	}

	public string GetID
	{
		get
		{
			return _getId;
		}
		set
		{
			_getId = value;
		}
	}

	public int ItemIndex
	{
		get
		{
			return _itemIndex;
		}
		set
		{
			_itemIndex = value;
		}
	}

	public DateTime LastTime
	{
		get
		{
			return _lastTime;
		}
		set
		{
			_lastTime = value;
		}
	}

	public DateTime PostTime
	{
		get
		{
			return _postTime;
		}
		set
		{
			_postTime = value;
		}
	}

	public int PutID
	{
		get
		{
			return _putId;
		}
		set
		{
			_putId = value;
		}
	}

	public int Size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}

	public Uri TextUrl
	{
		get
		{
			return _textUrl;
		}
		set
		{
			_textUrl = value;
		}
	}

	public string VolumeName
	{
		get
		{
			return _volumeName;
		}
		set
		{
			_volumeName = value;
		}
	}
}
