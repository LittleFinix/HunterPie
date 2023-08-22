using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Logger;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Debug;
using HunterPie.Features.Notification.ViewModels;
using HunterPie.GUI.Parts.Account.Views;
using HunterPie.GUI.ViewModels;
using HunterPie.Internal;
using HunterPie.UI.Controls.Notfication;
using HunterPie.Usecases;
using System;
using System.Threading.Tasks;

namespace HunterPie;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly RemoteAccountConfigService _remoteConfigService = new();
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        DataContext = new MainViewModel();
        
        Log.Info("Initializing HunterPie GUI");

        // Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = (int)ClientConfig.Config.Client.RenderFramePerSecond.Current });
    }

    protected override async void OnClosing(WindowClosingEventArgs e)
    {
        ConfigManager.SaveAll();

        if (!ClientConfig.Config.Client.EnableSeamlessShutdown)
        {
            NativeDialogResult result = await DialogManager.Info(
                Localization.FindString("Client", "Dialogs", "Dialog", "CONFIRMATION_TITLE_STRING"),
                Localization.FindString("Client", "Dialogs", "Dialog", "EXIT_CONFIRMATION_DESCRIPTION_STRING"),
                NativeDialogButtons.Accept | NativeDialogButtons.Cancel
            );

            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }
        }

        e.Cancel = true;

        await Dispatcher.UIThread.InvokeAsync(Hide);

        await _remoteConfigService.UploadClientConfig();

        InitializerManager.Unload();

        if (Application.Current.ApplicationLifetime is IControlledApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private async void OnInitialized(object sender, EventArgs e)
    {
        CheckIfHunterPiePathIsSafe();

        InitializerManager.InitializeGUI();

        InitializeDebugWidgets();

        SetupTrayIcon();
        SetupMainNavigator();
        SetupAccountEvents();

        await SetupPromoViewAsync();
    }

    private async Task SetupPromoViewAsync() => ViewModel.ShouldShowPromo = await AccountPromotionalUseCase.ShouldShow();

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Control && e.Key == Key.R)
            App.Restart();
    }

    private void InitializeDebugWidgets()
    {
        DebugWidgets.MockIfNeeded();
    }

    private void SetupTrayIcon()
    {
    }

    private void SetupMainNavigator()
    {
    }

    private void SetupAccountEvents()
    {
        AccountNavigationService.OnNavigateToSignIn += (_, __) => CreateSignFlowView(true);
        AccountNavigationService.OnNavigateToSignUp += (_, __) => CreateSignFlowView(false);
    }

    private void CreateSignFlowView(bool isLoggingIn)
    {
        AccountSignFlowView view = new();
        view.ViewModel.SelectedIndex = isLoggingIn ? 0 : 1;
        view.OnFormClose += OnSignFormClose;
        PART_SigninView.Content = view;
    }

    private void OnSignFormClose(object sender, EventArgs e)
    {
        if (sender is AccountSignFlowView view)
        {
            view.OnFormClose -= OnSignFormClose;
            PART_SigninView.Content = null;
        }
    }

    private void OnTrayShowClick(object sender, RoutedEventArgs e)
    {
        Show();
        WindowState = WindowState.Normal;
        _ = Focus();
    }

    private void OnTrayCloseClick(object sender, RoutedEventArgs e) => Close();

    public void StartGameClick()
    {
        Steam.RunGameBy(ClientConfig.Config.Client.DefaultGameType);
    }

    private void OnMouseDown(object sender, PointerPressedEventArgs e)
    {
        if (IsMouseOverElement(e, PART_HeaderBar))
            return;

        if (!IsMouseOverElement(e, PART_NotificationsPanel))
            PART_HeaderBar.ViewModel.IsNotificationsToggled = false;
    }

    private bool IsMouseOverElement(PointerPressedEventArgs args, FrameworkElement element)
    {
        Point points = args.GetPosition(element);
        return element.Bounds.Contains(points);
    }

    private void CheckIfHunterPiePathIsSafe()
    {
        bool isSafe = VerifyHunterPiePathUseCase.Invoke();

        if (isSafe)
            return;

        _ = DialogManager.Warn(
            "Unsafe path",
            "It looks like you're executing HunterPie directly from the zip file. Please extract it first before running the client.",
            NativeDialogButtons.Accept
        );

        if (Application.Current.ApplicationLifetime is IControlledApplicationLifetime lifetime)
            lifetime.Shutdown();
    }

    private void OnNotificationClick(object sender, RoutedEventArgs e)
    {
        if (sender is Push { DataContext: AppNotificationViewModel vm })
            vm.IsVisible = false;
    }
}
