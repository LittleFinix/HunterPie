using Avalonia.Input;
using Avalonia.Interactivity;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Controls.Settings.ViewModel;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : View<AccountViewModel>
{
    public AccountView()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        ViewModel.FetchAccountDetails();
    }

    private void OnAvatarGotKeyboardFocus(object sender, GotFocusEventArgs e) => ViewModel.IsAvatarClicked = true;
    private void OnAvatarLostKeyboardFocus(object sender, RoutedEventArgs e) => ViewModel.IsAvatarClicked = false;

    private void OnAvatarClick(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenAccountPreferences();
    }

    private void OnLoginClick(object sender, RoutedEventArgs e) => AccountNavigationService.NavigateToSignIn();
    private void OnRegisterClick(object sender, RoutedEventArgs e) => AccountNavigationService.NavigateToSignUp();
    private void OnLogoutClick(object sender, RoutedEventArgs e) => ViewModel.SignOut();
    private async void OnSettingsClick(object sender, RoutedEventArgs e)
    {
        ISettingElement[] accountConfig = await LocalAccountConfig.CreateAccountSettingsTab();

        var host = new SettingHost
        {
            DataContext = new SettingHostViewModel(accountConfig)
        };
        Navigator.Navigate(host);
    }
}
