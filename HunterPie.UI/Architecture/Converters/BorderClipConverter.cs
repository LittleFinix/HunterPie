using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

public class BorderClipConverter : IMultiValueConverter
{
    public object Convert(IList<object?> values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Count != 4 || values[0] is not double width || values[1] is not double height ||
            values[2] is not CornerRadius radius || values[3] is not Thickness borderThickness)
            return AvaloniaProperty.UnsetValue;

        if (width < double.Epsilon || height < double.Epsilon)
            return new RectangleGeometry(new Rect(0, 0, 0, 0));

        var clip = new RectangleGeometry(new Rect(
            radius.TopLeft + borderThickness.Top,
            radius.TopLeft + borderThickness.Left,
            width, height)
        );
        
        return clip;

    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}