using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.Features.Debug;

internal class DamageWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        OverlayConfig mockConfig = ClientConfig.Config.World.Overlay;

        if (ClientConfig.Config.Development.MockDamageWidget)
        {
            _ = WidgetManager.Register<MeterView, DamageMeterWidgetConfig>(
                new MeterView(mockConfig.DamageMeterWidget)
                {
                    DataContext = new MockMeterViewModel()
                }
            );
        }
    }
}
