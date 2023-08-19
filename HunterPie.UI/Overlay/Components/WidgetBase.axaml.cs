using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using HunterPie.Core.Logger;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Overlay.Enums;
using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Color = Avalonia.Media.Color;
#if DEBUG
using HunterPie.UI.Architecture.Graphs;
#endif

namespace HunterPie.UI.Overlay.Components;

/// <summary>
/// Interaction logic for WidgetBase.xaml
/// </summary>
public partial class WidgetBase : Window, INotifyPropertyChanged
{
    private DateTime _lastRender;
    private double _renderingTime;

    public double RenderingTime { get => _renderingTime; private set => SetValue(ref _renderingTime, value); }

#if DEBUG
    public SeriesCollection RenderSeries { get; private set; }
    private readonly List<ObservablePoint> _renderPoints = new();
#endif

    private IWidgetWindow _widget;
    public IWidgetWindow Widget
    {
        get => _widget;
        init
        {
            if (value != _widget)
            {
                _widget = value;

                if (_widget.Settings is null)
                    throw new NullReferenceException();
                
                _widget.OnWidgetTypeChange += OnWidgetTypeChange;

                // _widget.Settings.Position ??= new Position(0, 0);
                // _widget.Settings.Position.PropertyChanged += PositionOnPropertyChanged;
                Position = new PixelPoint((int)Widget.Settings.Position.X, (int)Widget.Settings.Position.Y);
                
                this.N(PropertyChanged);
            }
        }
    }

    public WidgetBase()
    {
#if DEBUG
        RenderSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(_renderPoints, "Render", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            .Build();
#endif

        InitializeComponent();
        DataContext = this;
    }

    private void PositionOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Position = new PixelPoint((int)Widget.Settings.Position.X, (int)Widget.Settings.Position.Y);
    }

    private int _counter = 0;
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

    public event PropertyChangedEventHandler PropertyChanged;

    private void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(property, value))
            return;

        property = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void OnWidgetTypeChange(object sender, WidgetType e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            switch (e)
            {
                case WidgetType.ClickThrough: HandleTransparencyFlag(true); break;
                case WidgetType.Window: HandleTransparencyFlag(false); break;
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
        Widget.Settings.Position = new Position(e.Point.X, e.Point.Y);
    }
}
