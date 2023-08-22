using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using Converter = System.Convert;

namespace HunterPie.UI.Architecture.Converters;
public class PercentageToRelativeSizeConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            if (values.Count < 3)
                throw new ArgumentException("there must be 3 double values");

            double current = ConverterHelper.ToDouble(values[0]);
            double max = Math.Max(1, ConverterHelper.ToDouble(values[1]));
            double relativeSize = ConverterHelper.ToDouble(values[2]);

            double result = current / max * relativeSize;

            return result < 0 ? 0 : result;
        }
        catch (Exception)
        {
            return 0.0;
        }
    }
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
