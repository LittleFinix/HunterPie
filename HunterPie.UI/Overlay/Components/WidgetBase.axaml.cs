using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Enums;
using LiveChartsCore.Defaults;
using System;
using System.ComponentModel;
#if DEBUG
using HunterPie.UI.Architecture.Graphs;
#endif

namespace HunterPie.UI.Overlay.Components;

/// <summary>
/// Interaction logic for WidgetBase.xaml
/// </summary>
public partial class WidgetBase : Window
{
    private DateTime _lastRender;
    private double _renderingTime;

    public static readonly DirectProperty<WidgetBase, double> RenderingTimeProperty =
        AvaloniaProperty.RegisterDirect<WidgetBase, double>(
            "RenderingTime", o => o.RenderingTime, (o, v) => o.RenderingTime = v);

    public double RenderingTime
    {
        get => _renderingTime;
        set => SetAndRaise(RenderingTimeProperty, ref _renderingTime, value);
    }

#if DEBUG
    public SeriesCollection RenderSeries { get; private set; }
    private readonly ChartSeries _renderPoints = new();
#endif

    private IWidgetWindow _widget;

    public IWidgetWindow Widget
    {
        get => _widget;
        init
        {
            if (value != _widget)
            {
                SetAndRaise(WidgetProperty, ref _widget, value);

                if (_widget.Settings is null)
                    throw new NullReferenceException();

                _widget.OnWidgetTypeChange += OnWidgetTypeChange;
                _widget.Settings.Position.PropertyChanged += PositionOnPropertyChanged;
                _widget.Settings.Enabled.PropertyChanged += EnabledOnPropertyChanged;

                SetPosition();
                IsVisible = _widget.Settings.Enabled;
            }
        }
    }

    private bool _isChangingPosition;

    private void PositionOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!_isChangingPosition)
            SetPosition();
    }

    private void SetPosition()
    {
        var area = ProcessManager.Current?.GameArea ?? Screens.Primary?.WorkingArea ?? default;

        Position = new PixelPoint((int)Widget.Settings.Position.X, (int)Widget.Settings.Position.Y)
                   + area.Position;
    }

    public static readonly DirectProperty<WidgetBase, IWidgetWindow> WidgetProperty =
        AvaloniaProperty.RegisterDirect<WidgetBase, IWidgetWindow>(
            "Widget", o => o.Widget);

    public WidgetBase()
    {
#if DEBUG
        RenderSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(_renderPoints, "Render", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            .Build();
#endif

        IsVisible = false;
        InitializeComponent();
        DataContext = this;

    }

    private void EnabledOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        IsVisible = Widget.Settings.Enabled;
    }

    private int _counter;

    private void OnRender(object sender, EventArgs e)
    {
        if (_counter >= 60)
        {
            RenderingTime = (DateTime.Now - _lastRender).TotalMilliseconds;
            ForceAlwaysOnTop();
            _counter = 0;
        }

        _lastRender = DateTime.Now;
        _counter++;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (e.CloseReason is not WindowCloseReason.ApplicationShutdown and not WindowCloseReason.OSShutdown)
        {
            Widget.Settings.Position.PropertyChanged -= PositionOnPropertyChanged;
            Widget.Settings.Enabled.PropertyChanged -= EnabledOnPropertyChanged;
            Widget.OnWidgetTypeChange -= OnWidgetTypeChange;
            base.OnClosing(e);
        }
    }

    private void SetWindowFlags()
    {
        Background = null;
        TransparencyBackgroundFallback = Brushes.Transparent;
        CanResize = false;
        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;

        switch (Widget.Type)
        {
            case WidgetType.Window:
                Focusable = true;
                break;

            case WidgetType.ClickThrough:
                Focusable = false;
                break;
        }
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => SetWindowFlags();

    internal void HandleTransparencyFlag(bool enableFlag)
    {
        if (enableFlag)
            Background = null;
        else
            Background = Brushes.Black;
    }

    private void ForceAlwaysOnTop()
    {
        Topmost = true;
    }

    private void OnWidgetTypeChange(object sender, WidgetType e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            switch (e)
            {
                case WidgetType.ClickThrough:
                    HandleTransparencyFlag(true);
                    break;
                case WidgetType.Window:
                    HandleTransparencyFlag(false);
                    break;
                default: throw new Exception("unreachable");
            }
        });
    }

    private void InputElement_OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        double step = 0.01 * (e.Delta.Y > 0 ? 1 : -1);

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            Widget.Settings.Opacity.Current += step;
        else
            Widget.Settings.Scale.Current += step;
    }

    private void WindowBase_OnPositionChanged(object? sender, PixelPointEventArgs e)
    {
        var area = ProcessManager.Current?.GameArea ?? Screens.Primary?.WorkingArea ?? default;
        PixelPoint point = e.Point;
        point -= area.Position;

        try
        {
            _isChangingPosition = true;
            Widget.Settings.Position.X = point.X;
            Widget.Settings.Position.Y = point.Y;
        }
        finally
        {
            _isChangingPosition = false;
        }
    }
}