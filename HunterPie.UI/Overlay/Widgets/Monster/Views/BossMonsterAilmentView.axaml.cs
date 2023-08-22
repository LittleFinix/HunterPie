using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

/// <summary>
/// Interaction logic for BossMonsterAilmentView.xaml
/// </summary>
public partial class BossMonsterAilmentView : UserControl
{
    public double Current
    {
        get => GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }
    
    public static readonly StyledProperty<double> CurrentProperty =
        AvaloniaProperty.Register<BossMonsterAilmentView, double>(nameof(Current));

    public double Max
    {
        get => GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }
    
    public static readonly StyledProperty<double> MaxProperty =
        AvaloniaProperty.Register<BossMonsterAilmentView, double>(nameof(Max));

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<BossMonsterAilmentView, bool>(
        "IsActive");

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public BossMonsterAilmentView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized() 
    {
        base.OnInitialized();

        this[!IsActiveProperty] = new Binding("IsActive");

        PseudoClasses.Set(":active", this.GetObservable(IsActiveProperty));
    }
}
