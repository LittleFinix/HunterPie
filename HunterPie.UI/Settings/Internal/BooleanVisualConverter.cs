using Avalonia.Layout;
using HunterPie.Core.Architecture;
using HunterPie.UI.Controls.Buttons;
using HunterPie.UI.Settings.Converter;
using System.Reflection;

namespace HunterPie.UI.Settings.Internal;

internal class BooleanVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var observable = (Observable<bool>)childInfo.GetValue(parent);
        Switch @switch = new()
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            // [Switch.IsActiveProperty] = observable
        };

        return @switch;
    }
}
