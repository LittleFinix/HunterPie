using Avalonia.Media;
using HunterPie.UI.Architecture;
using System.Collections.Generic;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;

public class StatusDetailsViewModel : ViewModel, ISectionControllable
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private double _upTime;
    public double UpTime { get => _upTime; set => SetValue(ref _upTime, value); }

    private bool _isToggled;
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }

    public List<AxisSection> Activations { get; init; } = new();

    private IBrush _color = Brushes.Transparent;
    public IBrush Color { get => _color; set => SetValue(ref _color, value); }
}
