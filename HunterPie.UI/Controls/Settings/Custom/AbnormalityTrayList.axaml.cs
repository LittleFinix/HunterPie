using Avalonia;
using Avalonia.Controls;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture.Navigator;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HunterPie.UI.Controls.Settings.Custom;

/// <summary>
/// Interaction logic for AbnormalityTrayList.xaml
/// </summary>
public partial class AbnormalityTrayList : UserControl, INotifyPropertyChanged
{
    private int _selectedIndex;

    public static readonly DirectProperty<AbnormalityTrayList, int> SelectedIndexProperty = AvaloniaProperty.RegisterDirect<AbnormalityTrayList, int>(
        "SelectedIndex", o => o.SelectedIndex, (o, v) => o.SelectedIndex = v);

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetAndRaise(SelectedIndexProperty, ref _selectedIndex, value);
    }
    
    private AbnormalityTrays ViewModel => (AbnormalityTrays)DataContext;

    public AbnormalityTrayList()
    {
        InitializeComponent();
    }

    private void OnAddTrayClick()
    {
        ViewModel.Trays.Add(new AbnormalityWidgetConfig { Name = $"Abnormality Tray {ViewModel.Trays.Count + 1}" });

        SelectedIndex = Math.Max(0, ViewModel.Trays.Count - 1);
    }

    private async Task OnRemoveTrayClick()
    {
        AbnormalityWidgetConfig? config = TryFetchConfig();

        if (config is null)
            return;

        NativeDialogResult confirmation = await DialogManager.Warn(
            Localization.FindString("Client", "Dialogs", "Dialog", "CONFIRMATION_TITLE_STRING"),
            Localization.FindString("Client", "Dialogs", "Dialog", "DELETE_CONFIRMATION_DESCRIPTION_STRING")
                .Replace("{Item}", $"{(string)config.Name}"),
            NativeDialogButtons.Accept | NativeDialogButtons.Cancel
        );

        if (confirmation != NativeDialogResult.Accept)
            return;

        ViewModel.Trays.RemoveAt(SelectedIndex);

        SelectedIndex = Math.Max(0, SelectedIndex - 1);
    }

    private void OnOpenConfigClick()
    {
        AbnormalityWidgetConfig? vm = TryFetchConfig();

        if (vm is null)
            return;

        var view = new AbnormalityWidgetConfigView(vm);
        Navigator.Navigate(view);
    }

    private AbnormalityWidgetConfig? TryFetchConfig()
    {
        try
        {
            return ViewModel.Trays[SelectedIndex];
        }
        catch
        {
            DialogManager.Error(
                title: "Not found",
                description: "Failed to find Abnormality Tray, if this issue keeps happening, try restarting HunterPie.",
                NativeDialogButtons.Accept
            );
            return null;
        }
    }
}
