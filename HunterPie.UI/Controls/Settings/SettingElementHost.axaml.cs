using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using HunterPie.UI.Controls.Settings.ViewModel;

namespace HunterPie.UI.Controls.Settings;

/// <summary>
/// Interaction logic for SettingElementHost.xaml
/// </summary>
public partial class SettingElementHost : UserControl
{
    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<SettingElementHost, string>(nameof(Text), "UNKNOWN_STRING");

    public string Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    
    public static readonly StyledProperty<string> DescriptionProperty =
        AvaloniaProperty.Register<SettingElementHost, string>(nameof(Description), "UNKNOWN_STRING_DESC");

    public ISettingElementType Hosted
    {
        get => GetValue(HostedProperty);
        set => SetValue(HostedProperty, value);
    }
    
    public static readonly StyledProperty<ISettingElementType> HostedProperty =
        AvaloniaProperty.Register<SettingElementHost, ISettingElementType>(nameof(Hosted));

    private object? _value;

    public static readonly DirectProperty<SettingElementHost, object?> ValueProperty = AvaloniaProperty.RegisterDirect<SettingElementHost, object?>(
        "Value", o => o.Value);

    public object? Value => _value;

    public SettingElementHost()
    {
        InitializeComponent();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!IsPointerOver)
            return;

        // Point pos = e.GetPosition(this);
        //
        // double left = pos.X - (PART_Highlight.Width / 2);
        // double top = 0;
        //
        // PART_Highlight.Margin = new Thickness(left, top, 0, 0);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == HostedProperty)
        {
            object? oldValue = _value;
            _value = change.NewValue is ISettingElementType o ? o.Information.GetValue(o.Parent) : null;
            
            RaisePropertyChanged(ValueProperty, oldValue, _value);
        }
        
        base.OnPropertyChanged(change);
    }
}
