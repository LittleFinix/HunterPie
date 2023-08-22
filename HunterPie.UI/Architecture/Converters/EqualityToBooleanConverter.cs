using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class EqualityToBooleanConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            throw new ArgumentException("Expected at least 2 parameters");

        return values[0].Equals(values[1]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}