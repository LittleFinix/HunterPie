using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class FloatFloorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            float f when float.IsNormal(f) => Math.Floor(f),
            double d when double.IsNormal(d) => Math.Floor(d),
            _ => 0
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
