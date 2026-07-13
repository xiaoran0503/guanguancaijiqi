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
		if (disposing)
		{
			icontainer_0?.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.ClientSize = new Size(469, 264);
		Font = new Font("宋体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "TaskForm";
		Text = "任务管理器";
		base.Load += TaskForm_Load;
		base.ResumeLayout(false);
	}

	private void TaskForm_Load(object sender, EventArgs e)
	{
	}
}
