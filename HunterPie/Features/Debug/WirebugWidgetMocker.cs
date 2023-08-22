using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;
using ClientConfig = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.Features.Debug;

internal class WirebugWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        OverlayConfig mockConfig = ClientConfig.Config.Rise.Overlay;

        if (ClientConfig.Config.Development.MockWirebugWidget)
        {
            _ = WidgetManager.Register<WirebugsView, WirebugWidgetConfig>(
                new WirebugsView(mockConfig.WirebugWidget)
                {
                    DataContext = new MockWirebugsViewModel()
                }
            );
        }
    }
}
