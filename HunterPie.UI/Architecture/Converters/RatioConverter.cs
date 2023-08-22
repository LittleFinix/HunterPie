using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;

public class RatioConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            return 0.0;

        try
        {
            double a = ConverterHelper.ToDouble(values[0]);
            double b = ConverterHelper.ToDouble(values[1]);

            return a / Math.Max(1, b);
        }
        catch
        {
            return 0.0;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
