using Avalonia.Input;
using Avalonia.Interactivity;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.GUI.Parts;

/// <summary>
/// Interaction logic for HeaderBar.xaml
/// </summary>
public partial class HeaderBar : View<HeaderBarViewModel>
{

    public HeaderBar()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (IsDesignMode)
            return;

        ViewModel.FetchSupporterStatus();

        HandleNotificationFeatureFlag();
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => ViewModel.CloseApplication();
    private void OnMinimizeButtonClick(object sender, EventArgs e) => ViewModel.MinimizeApplication();
    private void OnLeftMouseDown(object sender, PointerPressedEventArgs e) => ViewModel.DragApplication(e);
    private void OnNotificationsClick(object sender, RoutedEventArgs e) => ViewModel.IsNotificationsToggled = !ViewModel.IsNotificationsToggled;
    private void HandleNotificationFeatureFlag()
    {
        if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_IN_APP_NOTIFICATIONS))
            return;

        PART_NotificationButton.IsVisible = false;
    }
}
