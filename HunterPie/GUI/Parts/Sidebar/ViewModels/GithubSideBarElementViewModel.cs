using HunterPie.Core.Client.Localization;
using HunterPie.Core.System.Common;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;
internal class GithubSideBarElementViewModel : ISideBarElement
{
    private const string GITHUB_URL = "https://github.com/Haato3o/HunterPie-v2";

    public ImageSource Icon => Resources.Icon("ICON_GITHUB");

    public string Text => Localization.FindString("Client", "Tabs", "Tab", "GITHUB_STRING");

    public bool IsActivable => false;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public void ExecuteOnClick() => Launcher.Open(GITHUB_URL);
}
