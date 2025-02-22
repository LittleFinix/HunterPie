﻿using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class DynamicMonsterBarSizeConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is not double)
            return double.NaN;

        if (values[1] is not int)
            return double.NaN;

        double minWidth = (double)values[0];
        int monstersVisible = (int)values[1];

        double dynamicSize = minWidth + ((3 - monstersVisible) * (minWidth * 0.25));
        return dynamicSize;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
