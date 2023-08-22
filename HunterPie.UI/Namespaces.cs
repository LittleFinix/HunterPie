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
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

// ReSharper disable InconsistentNaming

public class Visibility
{
    public const bool Visible = true;
    public const bool Collapsed = false;
}

public abstract class DataTemplateSelector : IDataTemplate
{
    public abstract DataTemplate?
        SelectTemplate(object? item, DependencyObject container);
    
    public UIElement? Build(object? param) => SelectTemplate(param, null!)!.Build(param);

    public bool Match(object? data) => SelectTemplate(data, null!) is not null;
}

public abstract class ValidationRule : ValidationAttribute
{
    public abstract ValidationResult? Validate(object value, CultureInfo cultureInfo);
    
    public override bool IsValid(object? value)
    {
        return Validate(value!, CultureInfo.CurrentUICulture) == ValidationResult.Success;
    }
}
