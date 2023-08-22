using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace HunterPie.UI.Settings.Internal;

internal class EnumVisualConverter : IVisualConverter
{
    public DataTemplate EnumElementDataTemplate { get; } = Resources.Get<DataTemplate>("DATA_TEMPLATE_SETTINGS_ENUM_ELEMENT");

    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var observable = childInfo.GetValue(parent) as IObservable<object>;

        ObservableCollection<object> elements = new();

        foreach (object value in Enum.GetValues(childInfo.PropertyType.GenericTypeArguments[0]))
            elements.Add(value);

        ComboBox box = new()
        {
            ItemsSource = elements,
            ItemTemplate = EnumElementDataTemplate,
            MinHeight = 35,
            VerticalAlignment = VerticalAlignment.Center,
            [ComboBox.SelectedItemProperty] = childInfo.GetValue(parent) as IObservable<object>
        };

        return box;
    }
}
