using Avalonia.Data;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using RangeUI = HunterPie.UI.Controls.Sliders.Range;

namespace HunterPie.UI.Settings.Internal;

internal class RangeVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var range = (Range)childInfo.GetValue(parent);
        Binding currentBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Current));
        Binding maxBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Max));
        Binding minBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Min));
        Binding stepBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Step));

        RangeUI slider = new()
        {
            [RangeUI.ValueProperty] = currentBinding,
            [RangeUI.MaximumProperty] = maxBinding,
            [RangeUI.MinimumProperty] = minBinding,
            [RangeUI.ChangeProperty] = stepBinding
        };

        return slider;
    }
}
