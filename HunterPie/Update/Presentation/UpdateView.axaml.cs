using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace HunterPie.Update.Presentation;

/// <summary>
/// Interaction logic for UpdateView.xaml
/// </summary>
public partial class UpdateView : Window
{
    public static readonly StyledProperty<bool> IsMouseDownProperty = AvaloniaProperty.Register<UpdateView, bool>(
        nameof(IsMouseDown), false);

    public bool IsMouseDown
    {
        get => GetValue(IsMouseDownProperty);
        set => SetValue(IsMouseDownProperty, value);
    }

    public UpdateView()
    {
        InitializeComponent();
    }
    
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        IsMouseDown = true;
        BeginMoveDrag(e);
        IsMouseDown = false;
    }
}
