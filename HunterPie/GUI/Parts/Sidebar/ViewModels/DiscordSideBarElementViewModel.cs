using HunterPie.Core.Client.Localization;
using HunterPie.Core.System.Common;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class DiscordSideBarElementViewModel : ISideBarElement
{
    private const string DiscordUrl = "https://discord.gg/5pdDq4Q";

    public ImageSource Icon => Resources.Icon("ICON_DISCORD");

    public string Text => Localization.FindString("Client", "Tabs", "Tab", "DISCORD_STRING");

    public bool IsActivable => false;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public void ExecuteOnClick() => Launcher.Open(DiscordUrl);
}
