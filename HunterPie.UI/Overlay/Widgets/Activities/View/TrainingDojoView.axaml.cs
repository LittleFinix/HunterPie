using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Overlay.Widgets.Activities.View;

/// <summary>
/// Interaction logic for TrainingDojoView.xaml
/// </summary>
public partial class TrainingDojoView : UserControl
{
    public bool ShouldShowBuddies
    {
        get => GetValue(ShouldShowBuddiesProperty);
        set => SetValue(ShouldShowBuddiesProperty, value);
    }

    public static readonly StyledProperty<bool> ShouldShowBuddiesProperty =
        AvaloniaProperty.Register<TrainingDojoView, bool>(nameof(ShouldShowBuddies));

    public TrainingDojoView()
    {
        InitializeComponent();
    }

    private void OnClick() => ShouldShowBuddies = !ShouldShowBuddies;
}
