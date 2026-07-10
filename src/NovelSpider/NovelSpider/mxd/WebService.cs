using System;
using System.ComponentModel;

namespace NovelSpider.mxd;

[DesignerCategory("code")]
public class WebService
{
	public event GetIPCompletedEventHandler GetIPCompleted;
	public event HelloWorldCompletedEventHandler HelloWorldCompleted;
	public event SpiderEndCompletedEventHandler SpiderEndCompleted;
	public event SpiderRegCompletedEventHandler SpiderRegCompleted;
	public event SpiderVipIpDelCompletedEventHandler SpiderVipIpDelCompleted;
	public event SpiderVipIpGetCompletedEventHandler SpiderVipIpGetCompleted;
	public event SpiderVipIpSetCompletedEventHandler SpiderVipIpSetCompleted;

	public string Url { get; set; }
	public bool UseDefaultCredentials { get; set; }

	public WebService()
	{
		Url = "";
	}

	public void CancelAsync(object userState) { }

	public string GetIP() => "127.0.0.1";

	public string HelloWorld() => "OK";

	public void HelloWorldAsync() { }
	public void HelloWorldAsync(object userState) { }

	public string SpiderEnd(bool onoff) => "OK";
	public void SpiderEndAsync(bool onoff) { }
	public void SpiderEndAsync(bool onoff, object userState) { }

	public string SpiderReg() => "OK";
	public void SpiderRegAsync() { }
	public void SpiderRegAsync(object userState) { }

	public string SpiderVipIpDel(string text) => "OK";
	public void SpiderVipIpDelAsync(string text) { }
	public void SpiderVipIpDelAsync(string text, object userState) { }

	public string SpiderVipIpGet(string key) => "OK";
	public void SpiderVipIpGetAsync(string key) { }
	public void SpiderVipIpGetAsync(string key, object userState) { }

	public string SpiderVipIpSet(string text) => "OK";
	public void SpiderVipIpSetAsync(string text) { }
	public void SpiderVipIpSetAsync(string text, object userState) { }
}