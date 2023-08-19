using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.GUI.Parts.Account.ViewModels;

namespace HunterPie.GUI.Parts.Account.Views.Components;
/// <summary>
/// Interaction logic for AccountDetailsView.xaml
/// </summary>
public partial class AccountDetailsView : UserControl
{
    private AccountPreferencesViewModel ViewModel => (AccountPreferencesViewModel)DataContext;

    public AccountDetailsView()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e) => ViewModel.UploadAvatar();
}
