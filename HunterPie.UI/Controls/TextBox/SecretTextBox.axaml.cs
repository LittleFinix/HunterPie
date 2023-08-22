using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Controls.TextBox;

/// <summary>
/// Interaction logic for SecretTextBox.xaml
/// </summary>
public partial class SecretTextBox : UserControl
{
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<SecretTextBox, string>(nameof(Text), string.Empty);

    public bool IsContentVisible
    {
        get => GetValue(IsContentVisibleProperty);
        set => SetValue(IsContentVisibleProperty, value);
    }

    public static readonly StyledProperty<bool> IsContentVisibleProperty =
        AvaloniaProperty.Register<SecretTextBox, bool>(nameof(IsContentVisible));

    public SecretTextBox()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == TextProperty)
        {
            OnValueChange(TextProperty, change);            
        }
        
        base.OnPropertyChanged(change);
    }

    private void OnHideButtonClick() => IsContentVisible = !IsContentVisible;

    private void OnPasswordChanged(object sender, TextChangedEventArgs e) => Text = PART_PasswordBox.Text;

    private static void OnValueChange(object d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SecretTextBox secretTextBox)
        {
            string value = e.NewValue as string;
            if (secretTextBox.PART_PasswordBox.Text == value)
                return;

            secretTextBox.PART_PasswordBox.Text = value;
        }
    }
}
