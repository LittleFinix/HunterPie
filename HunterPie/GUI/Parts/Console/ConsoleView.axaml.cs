using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using System.Collections.Specialized;
using System.Linq;

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
            Dispatcher.UIThread.Invoke(() =>
            {
                PART_Items
                    .GetTemplateChildren()
                    .OfType<ScrollViewer>()
                    .FirstOrDefault(v => v.Name == "PART_Scroll")
                    ?.ScrollToEnd();
            }, DispatcherPriority.BeforeRender);
        }
    }
}
