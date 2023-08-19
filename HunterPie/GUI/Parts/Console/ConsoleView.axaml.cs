using Avalonia.Controls;
using System.Collections.Specialized;

namespace HunterPie.GUI.Parts.Console;

/// <summary>
/// Interaction logic for ConsoleView.xaml
/// </summary>
public partial class ConsoleView : UserControl
{
    public ConsoleView()
    {
        InitializeComponent();
        
        PART_Items.ItemsView.CollectionChanged += ItemsViewOnCollectionChanged;
    }

    private void ItemsViewOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Replace)
        {
            PART_Items
                .FindControl<ScrollViewer>("PART_Scroll")
                ?.ScrollToEnd();
        }
    }
}
