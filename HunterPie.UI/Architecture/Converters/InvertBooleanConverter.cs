using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;
public class InvertBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool boolean ? !boolean : throw new ArgumentException("value must be a boolean");
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is bool boolean ? !boolean : throw new ArgumentException("value must be a boolean");
}
