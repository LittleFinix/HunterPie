using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class RelativeSizeRectConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        double size = (parameter as double?) ?? 0;

        if (double.IsNaN(size))
            size = 0;
        
        Rect rect = new(0, 0, size, size);

        if (values[0] is double w && !double.IsNaN(w))
            rect = rect.WithWidth(w);
        
        if (values[1] is double h && !double.IsNaN(h))
            rect = rect.WithHeight(h);

        return rect;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
