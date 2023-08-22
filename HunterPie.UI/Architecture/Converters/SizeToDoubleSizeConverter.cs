using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class SizeToDoubleSizeConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count < 3)
            return new Thickness(0);

        double size = (double?)values[0] ?? 0.0;
        double parentSize = (double?)values[2] ?? 0.0;
        var parentMargin = (Thickness?)values[1] ?? new(0);

        if (double.IsNaN(size) || double.IsNaN(parentSize))
            return new Thickness(0);

        double diff = size - parentSize;

        return new Thickness(
              parentMargin.Left - (diff / 2),
              parentMargin.Top,
              parentMargin.Right,
              parentMargin.Bottom
        );
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
