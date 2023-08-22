using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using HunterPie.Core.Architecture;
using HunterPie.GUI.Parts.Sidebar.Service;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Sidebar;

/// <summary>
/// Interaction logic for SideBarContainer.xaml
/// </summary>
[PseudoClasses(":expanded")]
public partial class SideBarContainer : UserControl
{
    // TODO: Make a ViewModel for this
    private static readonly ObservableCollection<ISideBarElement> _elements = new();

    public ObservableCollection<ISideBarElement> Elements => _elements;

    public Observable<bool> IsMouseInside { get; } = false;

    public double ItemsHeight
    {
        get => GetValue(ItemsHeightProperty);
        set => SetValue(ItemsHeightProperty, value);
    }
    public static readonly StyledProperty<double> ItemsHeightProperty =
        AvaloniaProperty.Register<SideBarContainer, double>(nameof(ItemsHeight), 40.0);

    public Thickness SelectedButton
    {
        get => GetValue(SelectedButtonProperty);
        set => SetValue(SelectedButtonProperty, value);
    }
    public static readonly StyledProperty<Thickness> SelectedButtonProperty =
        AvaloniaProperty.Register<SideBarContainer, Thickness>(nameof(SelectedButton));

    public SideBarContainer()
    {
        HookEvents();

        InitializeComponent();

        DataContext = this;

        if (SideBarService.CurrentlySelected is not null)
            NavigateTo(SideBarService.CurrentlySelected);
    }

    private void HookEvents() => SideBarService.NavigateToElement += NavigateTo;

    public static void SetMenu(ISideBarElement[] menu) => Add(menu);

    public static void Add(params ISideBarElement[] elements)
    {
        foreach (ISideBarElement element in elements)
            _elements.Add(element);
    }

    private void NavigateTo(ISideBarElement element)
    {
        if (!element.IsActivable || !element.IsEnabled)
            return;

        int idx = Elements.IndexOf(element);

        (double l, double t, double r, double b) = PART_Selection.Margin;
        PART_Selection.Margin = new(l, idx* ItemsHeight, r, b);
    }

    private void InputElement_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        IsMouseInside.Value = true;
        PART_ButtonsContainer.IsHitTestVisible = true;
        PseudoClasses.Add(":expanded");
    }

    private void PART_ButtonsContainer_OnPointerExited(object? sender, PointerEventArgs e)
    {
        IsMouseInside.Value = false;
        PART_ButtonsContainer.IsHitTestVisible = false;
        PseudoClasses.Remove(":expanded");
    }
}
