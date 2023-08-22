using Avalonia.Media;
using Avalonia.Skia;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace HunterPie.UI.Architecture.Brushes;

public static class ColorFadeGradient
{
    private static readonly SKColor StartTransparency = new SKColor(0xC0, 0x0, 0x0, 0x0);
    private static readonly SKColor EndTransparency = new SKColor(0xFF, 0x0, 0x0, 0x0);

    private static Color Mix(SKColor a, Color b)
    {
        return new Color(
            (byte)(b.R - a.Red),
            (byte)(b.G - a.Green),
            (byte)(b.B - a.Blue),
            (byte)(b.A - a.Alpha)
        );
    }
    
    public static LinearGradientBrush MakeBrush(Color color)
    {
        var startColor = Mix(StartTransparency, color);
        var endColor = Mix(EndTransparency, color);

        return new LinearGradientBrush { GradientStops = { new(startColor, 0), new(endColor, 1), }, };
    }
    
    public static LinearGradientPaint FromColor(Color color)
    {
        var startColor = Mix(StartTransparency, color).ToSKColor();
        var endColor = Mix(EndTransparency, color).ToSKColor();

        LinearGradientPaint brush = new(startColor, endColor,
            new SKPoint(1, 0), new SKPoint(1, 1));
        
        return brush;
    }
}
