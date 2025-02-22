﻿using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class SizeAndCountToSizeConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not double size ||
            values[1] is not int count)
            return 0.0;

        return size / Math.Max(1, count);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}