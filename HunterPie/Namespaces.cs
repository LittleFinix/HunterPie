global using IChartSeries = LiveChartsCore.Kernel.Sketches.IChartSeries<LiveChartsCore.SkiaSharpView.Drawing.SkiaSharpDrawingContext>;
global using SeriesCollection = System.Collections.Generic.List<LiveChartsCore.Kernel.Sketches.IChartSeries<LiveChartsCore.SkiaSharpView.Drawing.SkiaSharpDrawingContext>>;
        
global using ChartSeries = System.Collections.Generic.List<LiveChartsCore.Defaults.ObservablePoint>;
global using ChartValues = System.Collections.Generic.List<LiveChartsCore.Defaults.ObservablePoint>;
global using LineSeries = LiveChartsCore.SkiaSharpView.LineSeries<LiveChartsCore.Defaults.ObservablePoint>;

global using DependencyObject = Avalonia.AvaloniaProperty;
global using DependencyPropertyChangedEventArgs = Avalonia.AvaloniaPropertyChangedEventArgs;
global using ImageSource = Avalonia.Media.IImage;
global using UIElement = Avalonia.Controls.Control;
global using FrameworkElement = Avalonia.Controls.Control;

global using Section = LiveChartsCore.SkiaSharpView.RectangularSection;
global using SectionsCollection = System.Collections.Generic.List<LiveChartsCore.SkiaSharpView.RectangularSection>;

global using AxisSection = LiveChartsCore.SkiaSharpView.Axis;