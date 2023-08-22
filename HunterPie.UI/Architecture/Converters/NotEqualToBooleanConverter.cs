using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;
public class NotEqualToBooleanConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            throw new ArgumentException("Expected 2 arguments");

        bool areEqual = EqualityComparer<object>.Default.Equals(values[0], values[1]);

        return !areEqual;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
