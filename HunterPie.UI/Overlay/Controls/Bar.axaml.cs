using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace HunterPie.UI.Overlay.Controls;

/// <summary>
/// Interaction logic for Bar.xaml
/// </summary>
public partial class Bar : UserControl
{

    // private readonly DoubleAnimation _cachedAnimation = new DoubleAnimation() { EasingFunction = new QuadraticEase(), Duration = new Duration(TimeSpan.FromMilliseconds(200)) };

    public double ActualValueDelayed
    {
        get => (double)GetValue(ActualValueDelayedProperty);
        set => SetValue(ActualValueDelayedProperty, value);
    }
    
    public static readonly StyledProperty<double> ActualValueDelayedProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(ActualValueDelayed));

    public double ActualValue
    {
        get => (double)GetValue(ActualValueProperty);
        set => SetValue(ActualValueProperty, value);
    }
    
    public static readonly StyledProperty<double> ActualValueProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(ActualValue));

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(Value));

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<Bar, double>(nameof(MaxValue));

    public IBrush ForegroundDelayed
    {
        get => (Brush)GetValue(ForegroundDelayedProperty);
        set => SetValue(ForegroundDelayedProperty, value);
    }

    public static readonly StyledProperty<IBrush> ForegroundDelayedProperty =
        AvaloniaProperty.Register<Bar, IBrush>(nameof(ForegroundDelayed));

    public bool MarkersVisibility
    {
        get => (bool)GetValue(MarkersVisibilityProperty);
        set => SetValue(MarkersVisibilityProperty, value);
    }
    public static readonly StyledProperty<bool> MarkersVisibilityProperty =
        AvaloniaProperty.Register<Bar, bool>(nameof(MarkersVisibility), Visibility.Visible);

    public Bar()
    {
        InitializeComponent();
    }

    private static void OnValueChange(object d, DependencyPropertyChangedEventArgs e)
    {
        var owner = d as Bar;

        if (owner.MaxValue == 0.0)
            return;

        double newValue = (owner.Width * ((double)e.NewValue / owner.MaxValue)) - 4;

        newValue = Math.Max(1.0, newValue);

        if (double.IsNaN(newValue))
            return;

        if (double.IsInfinity(owner.ActualValue) || double.IsInfinity(newValue))
            return;

        // DoubleAnimation animation = owner._cachedAnimation;
        // animation.From = owner.ActualValue;
        // animation.To = newValue;
        //
        // owner.BeginAnimation(Bar.ActualValueProperty, animation, HandoffBehavior.SnapshotAndReplace);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {

        double value = (e.NewSize.Width * (Value / MaxValue)) - (BorderThickness.Left + BorderThickness.Right);

        value = Math.Max(1.0, value);

        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            InvalidateVisual();
            return;
        }

        // DoubleAnimation smoothAnimation = _cachedAnimation;
        // smoothAnimation.From = value;
        // smoothAnimation.To = value;
        //
        // BeginAnimation(Bar.ActualValueProperty, smoothAnimation, HandoffBehavior.SnapshotAndReplace);
    }
}
