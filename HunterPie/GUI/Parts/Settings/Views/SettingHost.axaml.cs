using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.UI.Controls.TextBox.Events;

namespace HunterPie.GUI.Parts.Settings.Views;

/// <summary>
/// Interaction logic for SettingHost.xaml
/// </summary>
public partial class SettingHost : UserControl
{
    public SettingHostViewModel ViewModel => (SettingHostViewModel)DataContext;

    public SettingHost()
    {
        InitializeComponent();
    }

    private void OnRealTimeSearch(object sender, SearchTextChangedEventArgs e)
    {
        // ViewModel.SearchSetting(e.Text);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // ViewModel.FetchVersion();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        // ViewModel.UnhookEvents();
    }

    private void OnExecuteUpdateClick(object sender, RoutedEventArgs e) => ViewModel.ExecuteRestart();

    private void OnPanelLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void OnPanelDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void AnimatePanel(FrameworkElement element)
    {
        
    }
}
