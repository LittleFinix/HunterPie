using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Localization;
using HunterPie.GUI.Parts.Patches.Views;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class PatchNotesSideBarElementViewModel : Bindable, ISideBarElement
{
    private const string LAST_PATCH_NOTE_READ_KEY = "LastPatchNote";
    private bool _shouldNotify;

    public ImageSource Icon => Resources.Icon("ICON_DOCUMENTATION");
    public string Text => Localization.FindString("Client", "Tabs", "Tab", "PATCH_NOTES_STRING");
    public bool IsActivable => true;
    public bool IsEnabled => true;
    public bool ShouldNotify { get => _shouldNotify; private set => SetValue(ref _shouldNotify, value); }

    public void ExecuteOnClick()
    {
        var view = new PatchesView();

        Navigator.Navigate(view);
    }
}
