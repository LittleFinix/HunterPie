using HunterPie.Core.Client.Localization;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class PluginsSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_PLUGIN");

    public string Text => Localization.FindString("Client", "Tabs", "Tab", "PLUGINS_STRING");

    public bool IsActivable => true;

    public bool IsEnabled => false;

    public bool ShouldNotify => false;

    public void ExecuteOnClick()
    {

    }
}
