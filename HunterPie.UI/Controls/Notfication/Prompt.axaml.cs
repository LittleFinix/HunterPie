using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Controls.Notfication;
/// <summary>
/// Interaction logic for Prompt.xaml
/// </summary>
public partial class Prompt : UserControl
{
    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }

    // Using a DependencyProperty as the backing store for IconMargin.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<Thickness> IconMarginProperty =
        AvaloniaProperty.Register<Prompt, Thickness>(nameof(IconMargin), new Thickness(3));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<ImageSource> IconProperty =
        AvaloniaProperty.Register<Prompt, ImageSource>(nameof(Icon));

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<Prompt, string>(nameof(Message));



    public Prompt()
    {
        InitializeComponent();
    }
}
