using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Controls.Text;
/// <summary>
/// Interaction logic for LabeledText.xaml
/// </summary>
public partial class LabeledText : UserControl
{
    public string Label
    {
        get => (string)GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<LabeledText, string>(nameof(Label), "Label");

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<LabeledText, string>(nameof(Text), "Sample Text");

    public LabeledText()
    {
        InitializeComponent();
    }
}
