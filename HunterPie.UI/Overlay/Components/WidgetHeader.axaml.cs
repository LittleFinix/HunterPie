using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace HunterPie.UI.Overlay.Components;

/// <summary>
/// Interaction logic for WidgetHeader.xaml
/// </summary>
public partial class WidgetHeader : UserControl
{
    public WidgetBase Owner { get; private set; }

    public WidgetHeader()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e) => Owner.Widget.Settings.Initialize.Value = false;

    private void OnHideButtonClick(object sender, RoutedEventArgs e) =>
        Owner.Widget.Settings.Enabled.Value = !Owner.Widget.Settings.Enabled.Value;

    // private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (WidgetBase)Window.GetWindow(this);
    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (WidgetBase)Window.GetTopLevel(this);

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        Owner.BeginMoveDrag(e);
    }
}
