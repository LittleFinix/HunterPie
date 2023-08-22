using System;

namespace HunterPie.UI.Architecture.Converters;

public static class ConverterHelper
{
    public static double ToDouble(object? o)
    {
        if (o == DependencyObject.UnsetValue)
            return 0;
        if (o is not null)
            return Convert.ToDouble(o);
        return 0;
    }
}