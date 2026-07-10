using System;
using System.Reflection;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public static class DockThemeFactory
{
    public static ThemeBase CreateTheme(DockPanelSkin skin)
    {
        ThemeBase theme;

        try
        {
            // Use VS2015BlueTheme from DockPanelSuite.ThemeVS2015 NuGet package
            theme = new VS2015BlueTheme();
        }
        catch (TypeLoadException)
        {
            // Fallback: create DefaultTheme via reflection
            var defaultThemeType = typeof(DockPanel).Assembly.GetType("WeifenLuo.WinFormsUI.Docking.DefaultTheme");
            theme = (ThemeBase)Activator.CreateInstance(defaultThemeType, nonPublic: true);
        }

        // Set custom skin (ensure required sub-skins are populated)
        var skinProp = theme.GetType().GetProperty("Skin", BindingFlags.Public | BindingFlags.Instance);
        if (skinProp != null && skinProp.CanWrite)
        {
            if (skin.AutoHideStripSkin == null)
                skin.AutoHideStripSkin = new AutoHideStripSkin();
            if (skin.DockPaneStripSkin == null)
                skin.DockPaneStripSkin = new DockPaneStripSkin();
            skinProp.SetValue(theme, skin);
        }

        return theme;
    }
}
