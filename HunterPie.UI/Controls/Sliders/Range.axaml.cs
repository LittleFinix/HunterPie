using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using TB = Avalonia.Controls.TextBox;

namespace HunterPie.UI.Controls.Sliders;

/// <summary>
/// Interaction logic for Range.xaml
/// </summary>
public partial class Range : UserControl
{

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Range, double>(nameof(Value), 0.0);

    public double Maximum
    {
        get => (double)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<Range, double>(nameof(Maximum), 0.0);

    public double Minimum
    {
        get => (double)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }
    
    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<Range, double>(nameof(Minimum), 0.0);

    public double Change
    {
        get => (double)GetValue(ChangeProperty);
        set => SetValue(ChangeProperty, value);
    }
    
    public static readonly StyledProperty<double> ChangeProperty =
        AvaloniaProperty.Register<Range, double>(nameof(Change), 1.0);

    public Range()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && sender is TB textbox)
            UpdateBinding(textbox);

    }

    private void OnLostFocus(object sender, RoutedEventArgs e) => UpdateBinding(sender as TB);

    private static void UpdateBinding(TB textbox)
    {
        // @TODO figure this out
        // var binding = textbox.Bind(TB.TextProperty, new Binding("."));
        // binding.Dispose();
    }
}
