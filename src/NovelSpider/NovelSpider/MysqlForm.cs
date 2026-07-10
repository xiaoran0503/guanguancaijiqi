using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class MysqlForm : DockContent
{
	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button5;

	private ComboBox comboBox1;

	private ComboBox comboBox2;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private GroupBox groupBox4;

	private IContainer icontainer_0;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	public MysqlForm()
	{
		InitializeComponent();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.MysqlForm));
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label2 = new System.Windows.Forms.Label();
		this.button2 = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.button5 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.comboBox2 = new System.Windows.Forms.ComboBox();
		this.button3 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox4.SuspendLayout();
		base.SuspendLayout();
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.button2);
		this.groupBox1.Location = new System.Drawing.Point(12, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(564, 52);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "数据库升级(仅对杰奇系统有效)";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(87, 25);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(317, 12);
		this.label2.TabIndex = 4;
		this.label2.Text = "优化引导加快查询速度(初次使用优化一次之后不必再优化)";
		this.button2.Location = new System.Drawing.Point(6, 20);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 1;
		this.button2.Text = "优化";
		this.button2.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.button5);
		this.groupBox2.Controls.Add(this.button4);
		this.groupBox2.Location = new System.Drawing.Point(12, 70);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(206, 97);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "单次优化维护表";
		this.button5.Location = new System.Drawing.Point(110, 19);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(75, 23);
		this.button5.TabIndex = 1;
		this.button5.Text = "修复表";
		this.button5.UseVisualStyleBackColor = true;
		this.button4.Location = new System.Drawing.Point(6, 19);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(75, 23);
		this.button4.TabIndex = 0;
		this.button4.Text = "优化表";
		this.button4.UseVisualStyleBackColor = true;
		this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox3.Location = new System.Drawing.Point(12, 184);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(564, 172);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "进度显示";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(13, 26);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(17, 12);
		this.label1.TabIndex = 3;
		this.label1.Text = "每";
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(217, 26);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(125, 12);
		this.label3.TabIndex = 5;
		this.label3.Text = "执行优化和修复数据库";
		this.comboBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Items.AddRange(new object[6] { "凌晨1点", "凌晨2点", "凌晨3点", "凌晨4点", "凌晨5点", "凌晨6点" });
		this.comboBox1.Location = new System.Drawing.Point(114, 22);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(97, 20);
		this.comboBox1.TabIndex = 6;
		this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox4.Controls.Add(this.label5);
		this.groupBox4.Controls.Add(this.label4);
		this.groupBox4.Controls.Add(this.comboBox2);
		this.groupBox4.Controls.Add(this.button3);
		this.groupBox4.Controls.Add(this.button1);
		this.groupBox4.Controls.Add(this.comboBox1);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label1);
		this.groupBox4.Location = new System.Drawing.Point(224, 70);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(352, 97);
		this.groupBox4.TabIndex = 3;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "循环优化维护数据库";
		this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(139, 48);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(203, 12);
		this.label5.TabIndex = 11;
		this.label5.Text = "优化和修复时采集会暂停,但没有影响";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(81, 26);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(29, 12);
		this.label4.TabIndex = 10;
		this.label4.Text = "天的";
		this.comboBox2.FormattingEnabled = true;
		this.comboBox2.Items.AddRange(new object[6] { "1", "2", "3", "4", "5", "6" });
		this.comboBox2.Location = new System.Drawing.Point(36, 23);
		this.comboBox2.Name = "comboBox2";
		this.comboBox2.Size = new System.Drawing.Size(43, 20);
		this.comboBox2.TabIndex = 9;
		this.comboBox2.Text = "1";
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.Location = new System.Drawing.Point(267, 68);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(75, 23);
		this.button3.TabIndex = 8;
		this.button3.Text = "终止任务";
		this.button3.UseVisualStyleBackColor = true;
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(173, 68);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 7;
		this.button1.Text = "启动任务";
		this.button1.UseVisualStyleBackColor = true;
		base.ClientSize = new System.Drawing.Size(588, 375);
		base.Controls.Add(this.groupBox4);
		base.Controls.Add(this.groupBox3);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "MysqlForm";
		this.Text = "数据库维护";
		base.Load += new System.EventHandler(MysqlForm_Load);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		base.ResumeLayout(false);
	}

	private void MysqlForm_Load(object sender, EventArgs e)
	{
	}
}
