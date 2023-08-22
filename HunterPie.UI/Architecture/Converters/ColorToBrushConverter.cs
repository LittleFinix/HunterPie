using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;
using MediaBrushes = Avalonia.Media.Brushes;

namespace HunterPie.UI.Architecture.Converters;
public class ColorToBrushConverter : IValueConverter
{
    private static readonly IBrush Default = MediaBrushes.White;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color color)
            return Default;

        var brush = new SolidColorBrush(color);

        return brush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
