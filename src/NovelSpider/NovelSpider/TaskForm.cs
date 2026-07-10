using System;
using System.ComponentModel;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class TaskForm : DockContent
{
	private IContainer icontainer_0;

	public TaskForm()
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.TaskForm));
		base.SuspendLayout();
		base.ClientSize = new System.Drawing.Size(469, 264);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "TaskForm";
		this.Text = "任务管理器";
		base.Load += new System.EventHandler(TaskForm_Load);
		base.ResumeLayout(false);
	}

	private void TaskForm_Load(object sender, EventArgs e)
	{
	}
}
