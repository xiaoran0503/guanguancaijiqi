using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class CollectTasks : DockContent
{
	private Button button1;

	private TextBox FilterVolumeTextBox1;

	private GroupBox groupBox1;

	private IContainer icontainer_0;

	private Label label1;

	private MaskedTextBox maskedTextBox1;

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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.CollectTasks));
		base.SuspendLayout();
		base.ClientSize = new System.Drawing.Size(284, 262);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "CollectTasks";
		base.ResumeLayout(false);
	}

	private void method_0()
	{
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CollectTasks));
		button1 = new Button();
		maskedTextBox1 = new MaskedTextBox();
		label1 = new Label();
		groupBox1 = new GroupBox();
		FilterVolumeTextBox1 = new TextBox();
		groupBox1.SuspendLayout();
		SuspendLayout();
		button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
		button1.Location = new Point(330, 205);
		button1.Name = "button1";
		button1.Size = new Size(75, 23);
		button1.TabIndex = 6;
		button1.Text = "确认修改";
		button1.UseVisualStyleBackColor = true;
		maskedTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		maskedTextBox1.Location = new Point(81, 35);
		maskedTextBox1.Name = "maskedTextBox1";
		maskedTextBox1.Size = new Size(318, 21);
		maskedTextBox1.TabIndex = 7;
		label1.AutoSize = true;
		label1.Location = new Point(16, 38);
		label1.Name = "label1";
		label1.Size = new Size(53, 12);
		label1.TabIndex = 8;
		label1.Text = "章节名称";
		groupBox1.Controls.Add(FilterVolumeTextBox1);
		groupBox1.Location = new Point(12, 65);
		groupBox1.Name = "groupBox1";
		groupBox1.Size = new Size(393, 134);
		groupBox1.TabIndex = 9;
		groupBox1.TabStop = false;
		groupBox1.Text = "章节内容";
		FilterVolumeTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		FilterVolumeTextBox1.Location = new Point(6, 20);
		FilterVolumeTextBox1.Multiline = true;
		FilterVolumeTextBox1.Name = "FilterVolumeTextBox1";
		FilterVolumeTextBox1.ScrollBars = ScrollBars.Vertical;
		FilterVolumeTextBox1.Size = new Size(381, 108);
		FilterVolumeTextBox1.TabIndex = 1;
		base.ClientSize = new Size(417, 240);
		base.Controls.Add(groupBox1);
		base.Controls.Add(label1);
		base.Controls.Add(maskedTextBox1);
		base.Controls.Add(button1);
		Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
		base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		base.Name = "CollectTasks";
		Text = "章节内容";
		groupBox1.ResumeLayout(performLayout: false);
		groupBox1.PerformLayout();
		ResumeLayout(performLayout: false);
		PerformLayout();
	}
}
