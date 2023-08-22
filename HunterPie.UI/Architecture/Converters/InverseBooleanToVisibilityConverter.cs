using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool b ? b ? Visibility.Collapsed : Visibility.Visible : (object)Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
