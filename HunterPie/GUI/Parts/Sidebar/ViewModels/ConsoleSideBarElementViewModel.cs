using HunterPie.Core.Client.Localization;
using HunterPie.GUI.Parts.Console;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class ConsoleSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_CONSOLE");

    public string Text => Localization.FindString("Client", "Tabs", "Tab", "CONSOLE_STRING");

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public void ExecuteOnClick()
    {
        var console = new ConsoleView();
        Navigator.Navigate(console);
    }
}
