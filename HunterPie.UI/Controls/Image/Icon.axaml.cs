using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Controls.Image;
/// <summary>
/// Interaction logic for Icon.xaml
/// </summary>
public partial class Icon : UserControl
{

    public ImageSource Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
    public static readonly StyledProperty<ImageSource> ImageProperty =
        AvaloniaProperty.Register<Icon, ImageSource>(nameof(Image));

    public Icon()
    {
        InitializeComponent();
    }
}
