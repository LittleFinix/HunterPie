using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using System;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for TabItem.xaml
/// </summary>
public partial class TabItem : UserControl
{
    private readonly Animation? _rippleAnimation;
    
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<TabItem, string>(nameof(Title), "Header");

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<TabItem, string>(nameof(Description), "Header tooltip");

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public static readonly StyledProperty<ImageSource> IconProperty =
        AvaloniaProperty.Register<TabItem, ImageSource>(nameof(Icon));

    public TabItem()
    {
        InitializeComponent();
        _rippleAnimation = Resources["PART_RippleAnimation"] as Animation;
    }
    
    // protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    // {
    //     base.OnMouseLeftButtonDown(e);
    //
    //     double targetWidth = Math.Max(ActualWidth, ActualHeight) * 2;
    //     Point mousePosition = e.GetPosition(this);
    //     var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
    //     PART_Ripple.Margin = startMargin;
    //     (_rippleAnimation.Children[0] as DoubleAnimation).To = targetWidth;
    //     (_rippleAnimation.Children[1] as ThicknessAnimation).From = startMargin;
    //     (_rippleAnimation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - (targetWidth / 2), mousePosition.Y - (targetWidth / 2), 0, 0);
    //     PART_Ripple.BeginStoryboard(_rippleAnimation);
    // }
}
