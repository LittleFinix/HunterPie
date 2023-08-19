using Avalonia;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for Switch.xaml
/// </summary>
public partial class Switch : ClickableControl
{
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Switch, bool>(nameof(IsActive), false);

    public Switch()
    {
        InitializeComponent();
    }

    protected override void OnClickEvent()
    {
        base.OnClickEvent();

        IsActive = !IsActive;
    }
}
