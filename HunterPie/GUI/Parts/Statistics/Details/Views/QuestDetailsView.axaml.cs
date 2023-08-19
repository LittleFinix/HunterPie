using Avalonia.Controls;
using Avalonia.Interactivity;
using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Statistics.Details.Views;

/// <summary>
/// Interaction logic for QuestDetailsView.xaml
/// </summary>
public partial class QuestDetailsView : UserControl, IView<QuestDetailsViewModel>
{
    public QuestDetailsViewModel ViewModel => (QuestDetailsViewModel)DataContext;

    public QuestDetailsView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPreviousPage();

    private void OnMonsterPanelViewModelChanged(object sender, DependencyPropertyChangedEventArgs _) =>
        SetupView(sender);

    private void OnMonsterPanelLoaded(object sender, RoutedEventArgs _) =>
        SetupView(sender);

    private void SetupView(object obj)
    {
        if (obj is not MonsterDetailsView view)
            return;

        view.InitializeView();
        AnimatePanel(view);
    }

    private void AnimatePanel(FrameworkElement element)
    {
        
    }
}