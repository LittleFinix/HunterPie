using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;

namespace HunterPie.UI.Settings.Internal;

internal class FileSelectorVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var child = (IFileSelector)childInfo.GetValue(parent);

        Binding binding = VisualConverterHelper.CreateBinding(child, nameof(IFileSelector.Current));

        ComboBox ui = new()
        {
            ItemsSource = child.Elements,
            MinHeight = 35,
            VerticalAlignment = VerticalAlignment.Center,
            [ComboBox.SelectedValueProperty] = new Binding("Current") { Source = child }
        };

        return ui;
    }
}
