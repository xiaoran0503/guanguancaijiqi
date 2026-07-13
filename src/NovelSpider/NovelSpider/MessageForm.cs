using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NovelSpider;

public class MessageForm : Form
{
	private IContainer icontainer_0;

	public TextBox MessageText;

	public MessageForm()
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
		base.ClientSize = new Size(284, 262);
		base.Icon = AppIconProvider.Icon;
		base.Name = "MessageForm";
		base.ResumeLayout(false);
	}
}
