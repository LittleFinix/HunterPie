using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class MonsterAilmentIdToColorConverter : IValueConverter
{

    private static readonly SolidColorBrush _default = new(Color.FromRgb(0xFF, 0xFF, 0xFF));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var brush = Application.Current.FindResource($"COLOR_MONSTER_{value}") as Brush;
        return brush ?? _default;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
