using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.VisualTree;
using HunterPie.Core.Architecture;
using HunterPie.Core.Logger;
using HunterPie.UI.Architecture.Utils;
using System;
using System.Collections;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for ComboButton.xaml
/// </summary>
public partial class ComboButton : UserControl, ICommandSource
{
    public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register<ComboButton, ICommand>(
        "Command");

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register<ComboButton, object>(
        "CommandParameter");

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    
    public new object Content
    {
        get => (object)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public static new readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<ComboButton, object>(nameof(Content));

    public bool IsDropDownOpen
    {
        get => (bool)GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public static new readonly StyledProperty<bool> IsDropDownOpenProperty =
        ComboBox.IsDropDownOpenProperty.AddOwner<ComboButton>();
    
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static new readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        ComboBox.ItemsSourceProperty.AddOwner<ComboButton>();

    public object SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public static new readonly StyledProperty<object> SelectedValueProperty =
        ComboBox.SelectedValueProperty.AddOwner<ComboButton>();

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public static new readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        ComboBox.ItemTemplateProperty.AddOwner<ComboButton>();

    public ComboButton()
    {
        InitializeComponent();
    }

    public void CanExecuteChanged(object sender, EventArgs e) {}

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetTopLevel(this);
        
        if (parentWindow is null)
            return;

        IsDropDownOpen = false;
        parentWindow.LostFocus += (_, _) => IsDropDownOpen = false;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        finalSize = base.ArrangeOverride(finalSize);
        InnerWidth = finalSize.Width - 10;

        return finalSize;
    }
    
    public static readonly StyledProperty<double> InnerWidthProperty = AvaloniaProperty.Register<ComboButton, double>(
        "InnerWidth");

    public double InnerWidth
    {
        get => GetValue(InnerWidthProperty);
        set => SetValue(InnerWidthProperty, value);
    }
}
