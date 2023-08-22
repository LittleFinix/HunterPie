using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class RatioToArcSweepAngleConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        double max = 0.0;

        if (parameter is string s && s.ToLowerInvariant() == "reverse")
            max = 360.0;
        
        if (values.Count < 2)
            return max;

        try
        {
            double a = ConverterHelper.ToDouble(values[0]);
            double b = ConverterHelper.ToDouble(values[1]);

            return Math.Abs(max - (a / Math.Max(1, b)) * 360.0);
        }
        catch
        {
            return max;
        }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}