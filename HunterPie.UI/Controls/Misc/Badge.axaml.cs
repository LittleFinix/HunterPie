using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HunterPie.UI.Controls.Misc;

/// <summary>
/// Interaction logic for Badge.xaml
/// </summary>
public partial class Badge : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Badge, string>(nameof(Text), "New");

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for Foreground.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<Brush> ForegroundProperty =
        AvaloniaProperty.Register<Badge, Brush>(nameof(Foreground), new SolidColorBrush(Color.FromArgb(0xCC, 0x70, 0x00, 0x00)));

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<Brush> BackgroundProperty =
        AvaloniaProperty.Register<Badge, Brush>(nameof(Background), new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xA8, 0xA8)));

    public Badge()
    {
        InitializeComponent();
    }
}
