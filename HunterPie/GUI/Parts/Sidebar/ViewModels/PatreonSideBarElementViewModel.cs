using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.System.Common;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class PatreonSideBarElementViewModel : Bindable, ISideBarElement
{
    private const string PATREON_URL = "https://www.patreon.com/HunterPie";
    private const string PATREON_HAS_CLICKED = "HasClickedPatreon";
    private bool _shouldNotify;

    public ImageSource Icon => Resources.Icon("ICON_PATREON");
    public string Text => Localization.FindString("Client", "Tabs", "Tab", "PATREON_STRING");
    public bool IsActivable => false;
    public bool IsEnabled => true;
    public bool ShouldNotify { get => _shouldNotify; set => SetValue(ref _shouldNotify, value); }
    public void ExecuteOnClick()
    {
        Launcher.Open(PATREON_URL);
        RegistryConfig.Set(PATREON_HAS_CLICKED, true);

        ShouldNotify = false;
    }

    public PatreonSideBarElementViewModel()
    {
        if (!RegistryConfig.Exists(PATREON_HAS_CLICKED))
            RegistryConfig.Set(PATREON_HAS_CLICKED, false);

        bool hasClicked = RegistryConfig.Get<bool>(PATREON_HAS_CLICKED);

        ShouldNotify = !hasClicked;
    }
}
