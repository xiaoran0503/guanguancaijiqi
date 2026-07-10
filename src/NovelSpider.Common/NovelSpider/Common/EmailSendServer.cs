using System;
using System.Net;
using System.Net.Mail;
using NovelSpider.Config;

namespace NovelSpider.Common;

public class EmailSendServer
{
	public bool Boolean_0 { get; set; }

	public bool Enabled { get; set; }

	public int Port { get; set; }

	public string SenderEmail { get; set; }

	public string SmtpServer { get; set; }

	public string SmtpServerAccount { get; set; }

	public string SmtpServerPassword { get; set; }

	public EmailSendServer()
	{
		Enabled = true;
		SenderEmail = string.Empty;
		SmtpServer = string.Empty;
		SmtpServerAccount = string.Empty;
		SmtpServerPassword = string.Empty;
		Port = 25;
		Boolean_0 = false;
	}

	public string SendMail(EmailBase email)
	{
		MailMessage mailMessage = null;
		try
		{
			if (!SecurityUtil.IsEmail(email.ToEmail))
			{
				return "接收邮箱格式不正确";
			}
			mailMessage = new MailMessage(SenderEmail, email.ToEmail, email.Subject, email.Content);
			SmtpClient smtpClient = new SmtpClient(SmtpServer)
			{
				UseDefaultCredentials = false,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(SmtpServerAccount, SmtpServerPassword),
				EnableSsl = Boolean_0
			};
			mailMessage.IsBodyHtml = true;
			mailMessage.BodyEncoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk");
			smtpClient.Send(mailMessage);
			return "已发送";
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
		finally
		{
			if (mailMessage != null)
			{
				try
				{
					mailMessage.Dispose();
				}
				catch
				{
				}
			}
		}
	}
}
