using Avalonia.Controls;
using HunterPie.Core.Architecture;
using HunterPie.UI.Settings.Converter;
using System.Reflection;

namespace HunterPie.UI.Settings.Internal;

internal class StringVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var observable = (Observable<string>)childInfo.GetValue(parent);
        // Binding binding = VisualConverterHelper.CreateBinding(observable);
        TextBox textbox = new() { [TextBox.TextProperty] = observable };

        return textbox;
    }
}
