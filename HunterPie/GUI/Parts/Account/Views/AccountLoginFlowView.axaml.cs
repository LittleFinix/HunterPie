using Avalonia.Input;
using Avalonia.Interactivity;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Account.Views;
/// <summary>
/// Interaction logic for AccountLoginFlowView.xaml
/// </summary>
public partial class AccountLoginFlowView : View<AccountLoginFlowViewModel>
{
    public AccountLoginFlowView()
    {
        InitializeComponent();
    }

    private async void OnSignInClick(object sender, RoutedEventArgs e)
    {
        if (!await ViewModel.SignIn())
            return;
    }

    private async void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        _ = await ViewModel.SignIn();
    }

    private void OnForgotPasswordClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPasswordResetFlow();

    private void OnResendVerificationClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToAccountVerificationFlow();
}
