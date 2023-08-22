using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Controls.Misc;

/// <summary>
/// Interaction logic for Badge.xaml
/// </summary>
public partial class Badge : UserControl
{
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Badge, string>(nameof(Text), "New");

    public Badge()
    {
        InitializeComponent();
    }
}
