using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.ComponentModel;

namespace HunterPie.UI.Windows;

/// <summary>
/// Interaction logic for WindowHeader.xaml
/// </summary>
public partial class WindowChrome : UserControl, INotifyPropertyChanged
{
    private Window _owner;

    public static readonly DirectProperty<WindowChrome, Window> OwnerProperty = AvaloniaProperty.RegisterDirect<WindowChrome, Window>(
        "Owner", o => o.Owner);

    public Window Owner
    {
        get => _owner;
        private set
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (_owner is null)
                SetAndRaise(OwnerProperty, ref _owner!, value);
        }
    }

    public object Container
    {
        get => GetValue(ContainerProperty);
        set => SetValue(ContainerProperty, value);
    }
    
    public static readonly StyledProperty<object> ContainerProperty =
        AvaloniaProperty.Register<WindowChrome, object>(nameof(Container));

    public WindowChrome()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => Owner.Close();

    private void OnMinimizeButtonClick(object sender, EventArgs e) => Owner.WindowState = WindowState.Minimized;

    private void OnLeftMouseDown(object sender, PointerPressedEventArgs e) => Owner.BeginMoveDrag(e);

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (Window) TopLevel.GetTopLevel(this);
}
