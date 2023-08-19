using Avalonia;
using Avalonia.Media;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Notfication;

/// <summary>
/// Interaction logic for Push.xaml
/// </summary>
public partial class Push : ClickableControl
{
    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static new readonly StyledProperty<Brush> BackgroundProperty =
        AvaloniaProperty.Register<Push, Brush>(nameof(Background));

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    public static new readonly StyledProperty<Brush> ForegroundProperty =
        AvaloniaProperty.Register<Push, Brush>(nameof(Foreground));


    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<Push, string>(nameof(Message));

    public IImage Icon
    {
        get => (IImage)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<IImage> IconProperty =
        AvaloniaProperty.Register<Push, IImage>(nameof(Icon));

    public Push()
    {
        InitializeComponent();
    }
}
