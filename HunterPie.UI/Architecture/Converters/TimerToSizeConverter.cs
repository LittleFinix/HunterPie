﻿using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class TimerToSizeConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Count < 2)
            throw new Exception("Expected at least 2 arguments");

        double timer = (double)values[0];
        double maxTimer = (double)values[1];
        double maxSize = (double)values[2];

        return maxSize * (timer / maxTimer);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
