using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpConversion : DockContent
{
	private Button button_0;

	private Button button_1;

	private DateTimePicker dateTimePicker_0;

	private IContainer icontainer_0;

	private NumericUpDown numericUpDown_0;

	public HelpConversion()
	{
		InitializeComponent();
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		dateTimePicker_0.Value = FormatText.GetTime(Convert.ToInt32(numericUpDown_0.Value));
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		numericUpDown_0.Value = FormatText.GetTime(dateTimePicker_0.Value);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpConversion));
		this.dateTimePicker_0 = new System.Windows.Forms.DateTimePicker();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		base.SuspendLayout();
		this.dateTimePicker_0.CustomFormat = "yyyy/MM/dd HH:mm:ss";
		this.dateTimePicker_0.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateTimePicker_0.Location = new System.Drawing.Point(12, 12);
		this.dateTimePicker_0.Name = "dateTimePicker_0";
		this.dateTimePicker_0.Size = new System.Drawing.Size(156, 21);
		this.dateTimePicker_0.TabIndex = 0;
		this.button_0.Location = new System.Drawing.Point(12, 39);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 1;
		this.button_0.Text = "换算↑";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Location = new System.Drawing.Point(93, 39);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 2;
		this.button_1.Text = "换算↓";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.numericUpDown_0.Location = new System.Drawing.Point(12, 68);
		this.numericUpDown_0.Maximum = new decimal(new int[4] { 1215752191, 23, 0, 0 });
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(156, 21);
		this.numericUpDown_0.TabIndex = 3;
		base.ClientSize = new System.Drawing.Size(180, 102);
		base.Controls.Add(this.numericUpDown_0);
		base.Controls.Add(this.button_1);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.dateTimePicker_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "HelpConversion";
		this.Text = "换算";
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		base.ResumeLayout(false);
	}
}
