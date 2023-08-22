using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace HunterPie.UI.Overlay.Controls;

/// <summary>
/// Interaction logic for Bar.xaml
/// </summary>
public class Bar : ProgressBar
{
    public double ActualValueDelayed
    {
        get => GetValue(ActualValueDelayedProperty);
        set => SetValue(ActualValueDelayedProperty, value);
    }
    
    public static readonly StyledProperty<double> ActualValueDelayedProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(ActualValueDelayed));

    public double ActualValue
    {
        get => GetValue(ActualValueProperty);
        set => SetValue(ActualValueProperty, value);
    }
    
    public static readonly StyledProperty<double> ActualValueProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(ActualValue));

    public IBrush ForegroundDelayed
    {
        get => GetValue(ForegroundDelayedProperty);
        set => SetValue(ForegroundDelayedProperty, value);
    }

    public static readonly StyledProperty<IBrush> ForegroundDelayedProperty =
        AvaloniaProperty.Register<Bar, IBrush>(nameof(ForegroundDelayed));

    public bool MarkersVisibility
    {
        get => GetValue(MarkersVisibilityProperty);
        set => SetValue(MarkersVisibilityProperty, value);
    }
    public static readonly StyledProperty<bool> MarkersVisibilityProperty =
        AvaloniaProperty.Register<Bar, bool>(nameof(MarkersVisibility), Visibility.Visible);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }
}
