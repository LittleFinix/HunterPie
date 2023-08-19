using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using HunterPie.UI.Architecture.Extensions;
using System;
using System.ComponentModel;

namespace HunterPie.UI.Windows;

/// <summary>
/// Interaction logic for WindowHeader.xaml
/// </summary>
public partial class WindowHeader : UserControl
{
    public static readonly StyledProperty<bool> IsMouseDownProperty = AvaloniaProperty.Register<WindowHeader, bool>(
        "IsMouseDown");

    public bool IsMouseDown
    {
        get => GetValue(IsMouseDownProperty);
        set => SetValue(IsMouseDownProperty, value);
    }

    public static readonly StyledProperty<Window> OwnerProperty = AvaloniaProperty.Register<WindowHeader, Window>(
        "Owner");

    public Window Owner
    {
        get => GetValue(OwnerProperty);
        set => SetValue(OwnerProperty, value);
    }
    
    public WindowHeader()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e) => Owner.Close();

    private void OnMinimizeButtonClick(object sender, RoutedEventArgs e) => Owner.WindowState = WindowState.Minimized;

    private void OnLeftMouseDown(object sender, PointerPressedEventArgs e)
    {
        IsMouseDown = true;
        Owner.BeginMoveDrag(e);
        IsMouseDown = false;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (Window) Window.GetTopLevel(this);
}
