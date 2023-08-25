using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Data;

namespace HunterPie.UI.Overlay.Widgets.Monster.Views;

/// <summary>
/// Interaction logic for BossMonsterView.xaml
/// </summary>
[PseudoClasses(":alive", ":target", ":enraged", ":capturable")]
public partial class BossMonsterView : UserControl
{
    public static readonly StyledProperty<bool> IsTargetProperty = AvaloniaProperty.Register<BossMonsterView, bool>(
        "IsTarget");

    public bool IsTarget
    {
        get => GetValue(IsTargetProperty);
        set => SetValue(IsTargetProperty, value);
    }

    public static readonly StyledProperty<bool> IsEnragedProperty = AvaloniaProperty.Register<BossMonsterView, bool>(
        "IsEnraged");

    public bool IsEnraged
    {
        get => GetValue(IsEnragedProperty);
        set => SetValue(IsEnragedProperty, value);
    }

    public static readonly StyledProperty<bool> IsAliveProperty = AvaloniaProperty.Register<BossMonsterView, bool>(
        "IsAlive");

    public bool IsAlive
    {
        get => GetValue(IsAliveProperty);
        set => SetValue(IsAliveProperty, value);
    }

    public static readonly StyledProperty<bool> IsCapturableProperty = AvaloniaProperty.Register<BossMonsterView, bool>(
        "IsCapturable");

    public bool IsCapturable
    {
        get => GetValue(IsCapturableProperty);
        set => SetValue(IsCapturableProperty, value);
    }
    
    public BossMonsterView()
    {
        InitializeComponent();

        this[!IsAliveProperty] = new Binding("IsAlive");
        this[!IsTargetProperty] = new Binding("IsTarget");
        this[!IsEnragedProperty] = new Binding("IsEnraged");
        this[!IsCapturableProperty] = new Binding("IsCapturable");

        PseudoClasses.Set(":alive", this.GetObservable(IsAliveProperty));
        PseudoClasses.Set(":target", this.GetObservable(IsTargetProperty));
        PseudoClasses.Set(":enraged", this.GetObservable(IsEnragedProperty));
        PseudoClasses.Set(":capturable", this.GetObservable(IsCapturableProperty));
    }
}
