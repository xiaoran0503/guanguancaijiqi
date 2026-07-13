#nullable enable
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

internal sealed class DockWorkspaceService
{
	private readonly DockPanel dockPanel;

	public DockWorkspaceService(DockPanel dockPanel)
	{
		this.dockPanel = dockPanel ?? throw new ArgumentNullException(nameof(dockPanel));
	}

	public DockContent? ActiveContent { get; private set; }

	public void ShowDocument(DockContent content)
	{
		if (content == null || content.IsDisposed)
		{
			return;
		}

		if (content.MdiParent != null)
		{
			content.MdiParent = null;
		}

		if (content.DockPanel == dockPanel)
		{
			content.Show();
			content.Focus();
			ActiveContent = content;
			return;
		}

		content.Show(dockPanel, DockState.Document);
		ActiveContent = content;
	}
}
