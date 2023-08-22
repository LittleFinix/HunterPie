using Avalonia.Media;
using Avalonia.Skia;
using HunterPie.UI.Architecture.Brushes;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Collections.Generic;

namespace HunterPie.UI.Architecture.Graphs;

public class LinearSeriesCollectionBuilder
{
    private readonly SeriesCollection _instance = new();

    public LinearSeriesCollectionBuilder AddSeries(IEnumerable<ObservablePoint> points, string title, Color color)
    {
        var series = new LineSeries
        {
            // Title = title,
            Stroke = new SolidColorPaint(color.ToSKColor())
            {
              StrokeThickness  = 2,
            },
            Fill = ColorFadeGradient.FromColor(color),
            LineSmoothness = 0.7,
            Values = points
        };

        _instance.Add(series);

        return this;
    }

    public SeriesCollection Build() => _instance;

}
