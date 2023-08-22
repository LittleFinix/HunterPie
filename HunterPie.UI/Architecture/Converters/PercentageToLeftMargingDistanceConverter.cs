using Avalonia;
using Avalonia.Data.Converters;
using HunterPie.UI.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class PercentageToLeftMarginDistanceConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var oldThickness = parameter?.ToThickness() ?? new Thickness();

        if (values[0] is not double width)
            return oldThickness;
        
        if (values[1] is not double percentage)
            return oldThickness;

        if (double.IsNaN(width))
            return oldThickness;

        double left = width * percentage;

        if (double.IsNaN(left) || double.IsInfinity(left))
            return oldThickness;

        return oldThickness + new Thickness(left, 0, 0, 0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
