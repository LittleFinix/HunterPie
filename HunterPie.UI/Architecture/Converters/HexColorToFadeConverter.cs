using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using System;
using System.Globalization;

namespace HunterPie.UI.Architecture.Converters;

/// <summary>
/// Converts a Hex Color string (e.g: #FF000102) into a fade
/// it's used by the <seealso cref="PlayerDamageView"/>
/// </summary>
public class HexColorToFadeConverter : IValueConverter
{

    private static readonly LinearGradientBrush _defaultBrush = new()
    {
        GradientStops =
        {
            new GradientStop(Color.FromArgb(0, 0, 0, 0), 0),
            new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 1),
            new GradientStop(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF), 0.958),
            new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.955),
            new GradientStop(Color.FromArgb(0x50, 0xFF, 0xFF, 0xFF), 0.952),
        }, 
        
        StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Absolute),
        EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Absolute)
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string val)
        {
            var baseBrush = Color.Parse(val);
            var fadeBrush = Color.FromArgb((byte)(baseBrush.A - 0xAF), baseBrush.R, baseBrush.G, baseBrush.B);

            LinearGradientBrush gradientBrush = new()
            {
                EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Absolute),
                StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Absolute)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0));
            gradientBrush.GradientStops.Add(new GradientStop(baseBrush, 1));
            gradientBrush.GradientStops.Add(new GradientStop(baseBrush, 0.958));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 0.955));
            gradientBrush.GradientStops.Add(new GradientStop(fadeBrush, 0.952));

            return gradientBrush;
        }

        return _defaultBrush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
