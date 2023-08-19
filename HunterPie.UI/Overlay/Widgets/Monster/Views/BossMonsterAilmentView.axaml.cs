using Avalonia;
using Avalonia.Controls;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

/// <summary>
/// Interaction logic for BossMonsterAilmentView.xaml
/// </summary>
public partial class BossMonsterAilmentView : UserControl
{
    public double Current
    {
        get => (double)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }
    
    public static readonly StyledProperty<double> CurrentProperty =
        AvaloniaProperty.Register<BossMonsterAilmentView, double>(nameof(Current), 0.0);

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }
    
    public static readonly StyledProperty<double> MaxProperty =
        AvaloniaProperty.Register<BossMonsterAilmentView, double>(nameof(Max), 0.0);

    public BossMonsterAilmentView()
    {
        InitializeComponent();
    }
}
