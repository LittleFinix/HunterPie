using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace HunterPie.UI.Controls.Buttons;
/// <summary>
/// Interaction logic for Pagination.xaml
/// </summary>
public partial class Pagination : UserControl
{
    public static readonly RoutedEvent PageUpdateEvent = RoutedEvent.Register<Pagination, RoutedEventArgs>(nameof(PageUpdate), RoutingStrategies.Direct);

    public event EventHandler<RoutedEventArgs> PageUpdate
    {
        add => AddHandler(PageUpdateEvent, value);
        remove => RemoveHandler(PageUpdateEvent, value);
    }

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<int> CurrentPageProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(CurrentPage), 0);

    public int LastPage
    {
        get => (int)GetValue(LastPageProperty);
        set => SetValue(LastPageProperty, value);
    }

    // Using a DependencyProperty as the backing store for LastPage.  This enables animation, styling, binding, etc...
    public static readonly StyledProperty<int> LastPageProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(LastPage), 0);

    public Pagination()
    {
        InitializeComponent();
    }

    private void OnPreviousPageClick(object sender, RoutedEventArgs e)
    {
        CurrentPage -= 1;
        RaiseEvent(new RoutedEventArgs(PageUpdateEvent, this));
    }

    private void OnNextPageClick(object sender, RoutedEventArgs e)
    {
        CurrentPage += 1;
        RaiseEvent(new RoutedEventArgs(PageUpdateEvent, this));
    }
}
