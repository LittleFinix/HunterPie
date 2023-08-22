using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class MaxDeathsToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int maxdeaths = (int)value;
        return maxdeaths == 0
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
