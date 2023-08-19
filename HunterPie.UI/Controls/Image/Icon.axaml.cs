using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HunterPie.UI.Controls.Image;
/// <summary>
/// Interaction logic for Icon.xaml
/// </summary>
public partial class Icon : UserControl
{

    public ImageSource Image
    {
        get => (ImageSource)GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
    public static readonly StyledProperty<ImageSource> ImageProperty =
        AvaloniaProperty.Register<Icon, ImageSource>(nameof(Image));

    public new IBrush Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
    
    public static readonly StyledProperty<IBrush> ForegroundProperty =
        AvaloniaProperty.Register<Icon, IBrush>(nameof(Foreground));
    
    public Icon()
    {
        InitializeComponent();
    }
}
