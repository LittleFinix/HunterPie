using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Damage;

internal class MemberInfo
{
    public PlayerViewModel ViewModel { get; init; }
    public IChartSeries Series { get; init; }
    public double JoinedAt { get; set; }
    public double FirstHitAt { get; set; } = -1;
}
