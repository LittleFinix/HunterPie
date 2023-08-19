using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.GUI.Parts.Notifications.ViewModels;

namespace HunterPie.GUI.Parts.Notifications.Views;

/// <summary>
/// Interaction logic for NotificationsPanelView.xaml
/// </summary>
public partial class NotificationsPanelView : UserControl
{
    private NotificationsPanelViewModel ViewModel => (NotificationsPanelViewModel)DataContext;

    public NotificationsPanelView()
    {
        DataContext = new NotificationsPanelViewModel();
        InitializeComponent();
    }
    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Design.IsDesignMode)
            return;

        await ViewModel.FetchNotifications();
    }
}
