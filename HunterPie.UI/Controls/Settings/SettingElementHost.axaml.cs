using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace HunterPie.UI.Controls.Settings;

/// <summary>
/// Interaction logic for SettingElementHost.xaml
/// </summary>
public partial class SettingElementHost : UserControl
{
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<SettingElementHost, string>(nameof(Text), "UNKNOWN_STRING");

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<SettingElementHost, string>(nameof(Description), "UNKNOWN_STRING_DESC");

    public FrameworkElement Hosted
    {
        get => (FrameworkElement)GetValue(HostedProperty);
        set => SetValue(HostedProperty, value);
    }
    
    public static readonly StyledProperty<FrameworkElement> HostedProperty =
        AvaloniaProperty.Register<SettingElementHost, FrameworkElement>(nameof(Hosted));

    public SettingElementHost()
    {
        InitializeComponent();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!IsPointerOver)
            return;

        Point pos = e.GetPosition(this);

        double left = pos.X - (PART_Highlight.Width / 2);
        double top = 0;

        PART_Highlight.Margin = new Thickness(left, top, 0, 0);
    }
}
