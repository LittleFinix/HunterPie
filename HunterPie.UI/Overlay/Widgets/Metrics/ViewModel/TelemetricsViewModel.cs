using Avalonia.Media;
using HunterPie.Core.Architecture;
using HunterPie.Core.Extensions;
using HunterPie.UI.Architecture.Graphs;
using LiveChartsCore.Defaults;
using System;
using System.Diagnostics;
using System.Timers;

namespace HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;

public class TelemetricsViewModel : Bindable
{

    private long _memoryUsage;
    private float _cpuUsage;
    private int _threads;
    private readonly Timer _dispatcher;

    public long Memory
    {
        get => _memoryUsage;
        set => SetValue(ref _memoryUsage, value);
    }

    public float CPU
    {
        get => _cpuUsage;
        set => SetValue(ref _cpuUsage, value);
    }

    public int Threads
    {
        get => _threads;
        set => SetValue(ref _threads, value);
    }

    public SeriesCollection CPUSeries { get; private set; }
    public SeriesCollection RAMSeries { get; private set; }

    private readonly ChartSeries CPUPoints = new();
    private readonly ChartSeries WorkingSetPoints = new();
    private readonly ChartSeries PrivateSetPoints = new();

    public Func<double, string> BytesFormatter { get; } =
        value => ((long)value).FormatBytes();

    public Func<double, string> PercentageFormatter { get; } =
        value => $"{value:0.0}%";

    public TelemetricsViewModel()
    {
        // TODO: Make the graphs easier to code
        CPUSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(CPUPoints, "CPU", Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
            .Build();

        RAMSeries = new LinearSeriesCollectionBuilder()
            .AddSeries(WorkingSetPoints, "Working", Color.Parse("#5C97F8"))
            .AddSeries(PrivateSetPoints, "Private", Color.Parse("#7B65F0"))
            .Build();

        _dispatcher = new Timer(5000)
        {
            AutoReset = true
        };
        _dispatcher.Elapsed += UpdateInformation;
        _dispatcher.Start();
    }

    private double start = double.MaxValue;
    public void UpdateInformation(object source, ElapsedEventArgs e)
    {
        double current = TimeSpan.FromTicks(e.SignalTime.Ticks).TotalSeconds;
        start = Math.Min(start, current);

        double elapsed = current - start;

        using (var self = Process.GetCurrentProcess())
        {
            Memory = self.WorkingSet64 / 1024 / 1024;
            Threads = self.Threads.Count;
            WorkingSetPoints.Add(new ObservablePoint(elapsed, self.WorkingSet64));
            PrivateSetPoints.Add(new ObservablePoint(elapsed, self.PrivateMemorySize64));
        }

        CPU = (float)((Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds / Environment.ProcessorCount) / current);
        CPUPoints.Add(new ObservablePoint(elapsed, CPU));

        if (CPUPoints.Count > 50)
        {
            CPUPoints.RemoveAt(0);
            WorkingSetPoints.RemoveAt(0);
            PrivateSetPoints.RemoveAt(0);
        }
    }

    public void ExecuteGarbageCollector() => GC.Collect();

}
