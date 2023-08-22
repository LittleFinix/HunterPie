using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for BannerCard.xaml
/// </summary>
public partial class BannerCard : UserControl
{

    public string Banner
    {
        get => GetValue(BannerProperty);
        set => SetValue(BannerProperty, value);
    }

    // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> BannerProperty =
        AvaloniaProperty.Register<BannerCard, string>(nameof(Banner));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<BannerCard, string>(nameof(Title));

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<BannerCard, string>(nameof(Description));

    public string Link
    {
        get => GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    // Using a DependencyProperty as the backing store for Link.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> LinkProperty =
        AvaloniaProperty.Register<BannerCard, string>(nameof(Link));

    public BannerCard()
    {
        InitializeComponent();
    }

    private void OnCardClick(object sender, RoutedEventArgs e) => Process.Start("explorer", Link);
}
