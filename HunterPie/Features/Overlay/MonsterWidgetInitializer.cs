﻿using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster;

namespace HunterPie.Features.Overlay;

internal class MonsterWidgetInitializer : IWidgetInitializer
{
    private MonsterWidgetContextHandler? _handler;

    public void Load(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.BossesWidget.Initialize)
            return;

        _handler = new MonsterWidgetContextHandler(context);
    }
    
    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}
