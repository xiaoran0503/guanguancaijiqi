using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelVip.mxd;

namespace NovelVip;

public class form1 : Form
{
	private Button button1;

	private Button button2;

	private IContainer components;

	private WebService service;

	private TextBox textBox_0;

	private bool web_f;

	private string web_i;

	private bool web_o;

	private bool web_v;

	public form1()
	{
		components = null;
		web_i = "本机";
		web_o = true;
		web_v = true;
		web_f = false;
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		button1.Enabled = false;
		button2.Enabled = false;
		if (web_f)
		{
			textBox_0.Text += "\r\n";
			textBox_0.Text += "高级功能：已开启，无需重复开启。";
			textBox_0.Text += "\r\n";
			button1.Enabled = true;
			button2.Enabled = true;
			return;
		}
		web_o = true;
		web_v = true;
		web_f = true;
		textBox_0.Text += "\r\n";
		textBox_0.Text += "高级功能：已开启。";
		textBox_0.Text += "\r\n";
		button1.Enabled = true;
		button2.Enabled = true;
	}

	private void button2_Click(object sender, EventArgs e)
	{
		button1.Enabled = false;
		button2.Enabled = false;
		if (!web_f)
		{
			textBox_0.Text += "\r\n";
			textBox_0.Text += "高级功能：当前未开启，无需重复注销。";
			textBox_0.Text += "\r\n";
			button1.Enabled = true;
			button2.Enabled = true;
			return;
		}
		web_f = false;
		textBox_0.Text += "\r\n";
		textBox_0.Text += "高级功能：已注销。";
		textBox_0.Text += "\r\n";
		button1.Enabled = true;
		button2.Enabled = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		string text = "授权验证已移除，功能已全量开放。";
		DateTime now = DateTime.Now;
		textBox_0.Text += "\r\n";
		textBox_0.Text = textBox_0.Text + "当前IP：" + web_i;
		textBox_0.Text += "\r\n";
		textBox_0.Text += "普通功能：可用";
		textBox_0.Text += "\r\n";
		textBox_0.Text += "高级功能：可用";
		textBox_0.Text += "\r\n";
		textBox_0.Text = textBox_0.Text + "数据验证：" + now.ToString();
		textBox_0.Text += "\r\n";
		textBox_0.Text = textBox_0.Text + "版本消息：" + text;
		textBox_0.Text += "\r\n";
	}

	private void InitializeComponent()
	{
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(12, 12);
		this.textBox_0.Multiline = true;
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_0.Size = new System.Drawing.Size(260, 195);
		this.textBox_0.TabIndex = 1;
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.button1.Location = new System.Drawing.Point(12, 213);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(126, 37);
		this.button1.TabIndex = 2;
		this.button1.Text = "开启高级功能";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.button2.Location = new System.Drawing.Point(144, 213);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(126, 37);
		this.button2.TabIndex = 3;
		this.button2.Text = "注销高级功能";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(284, 262);
		base.Controls.Add(this.button2);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.textBox_0);
		base.Name = "Form1";
		this.Text = "關關采集器高级功能精灵";
		base.Load += new System.EventHandler(Form1_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
