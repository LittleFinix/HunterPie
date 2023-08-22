using Avalonia.Media;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Controls;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ResourcesService = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.Overlay.Widgets.Player.Views;

/// <summary>
/// Interaction logic for PlayerHudView.xaml
/// </summary>
public partial class PlayerHudView : View<PlayerHudViewModel>, IWidget<PlayerHudWidgetConfig>, IWidgetWindow
{
    private readonly Brush _defaultHealthBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FOREGROUND");
    private readonly Brush _defaultStaminaBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_STAMINA_FOREGROUND");
    private readonly Brush _defaultRecoverableBrush = ResourcesService.Get<Brush>("WIDGET_PLAYER_RECOVERABLE_FOREGROUND");

    private readonly AbnormalityCategory[] _healthCategoriesPriority = { AbnormalityCategory.Effluvia, AbnormalityCategory.Poison, AbnormalityCategory.Fire, AbnormalityCategory.Bleed };
    private readonly AbnormalityCategory[] _staminaCategoriesPriority = { AbnormalityCategory.Water, AbnormalityCategory.Ice };
    private readonly AbnormalityCategory[] _recoverableCategoriesPriority = { AbnormalityCategory.NaturalHealing };

    private readonly Dictionary<AbnormalityCategory, Brush> _abnormalityColors = new Dictionary<AbnormalityCategory, Brush>
    {
        { AbnormalityCategory.Fire, ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FIRE_FOREGROUND") },
        { AbnormalityCategory.Poison, ResourcesService.Get<Brush>("WIDGET_PLAYER_POISON_FOREGROUND") },
        { AbnormalityCategory.Bleed, ResourcesService.Get<Brush>("WIDGET_PLAYER_BLEED_FOREGROUND") },
        { AbnormalityCategory.Effluvia, ResourcesService.Get<Brush>("WIDGET_PLAYER_EFFLUVIA_FOREGROUND") },
        { AbnormalityCategory.Ice, ResourcesService.Get<Brush>("WIDGET_PLAYER_ICE_FOREGROUND") },
        { AbnormalityCategory.Water, ResourcesService.Get<Brush>("WIDGET_PLAYER_WATER_FOREGROUND") },
        { AbnormalityCategory.NaturalHealing, ResourcesService.Get<Brush>("WIDGET_PLAYER_NATURAL_HEAL_FOREGROUND") },
    };

    public PlayerHudWidgetConfig Settings { get; }

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public string Title => "Player Widget";

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    public PlayerHudView()
    {
        InitializeComponent();
    }

    public PlayerHudView(PlayerHudWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    protected override void Initialize()
    {
        HookEvents();
    }

    private void HookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged += OnActiveAbnormalitiesCollectionChanged;
    }

    private void UnhookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged -= OnActiveAbnormalitiesCollectionChanged;
    }

    private void ResetAnimation(Bar bar, Brush defaultBrush)
    {
        bar.Foreground = defaultBrush;
    }
    
    private void AnimateBar(Bar bar, Brush brush)
    {
        bar.Foreground = brush;
    }

    private void HandleAbnormalityCategoryChange() =>
        UIThread.Invoke(() =>
        {

            var categories = ViewModel.ActiveAbnormalities.ToHashSet();

            AbnormalityCategory healthAbnormality = categories.FindPriority(_healthCategoriesPriority);
            AbnormalityCategory staminaAbnormality = categories.FindPriority(_staminaCategoriesPriority);
            AbnormalityCategory recoverableAbnormality = categories.FindPriority(_recoverableCategoriesPriority);

            if (healthAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_HealthBar, _defaultHealthBrush);
            else
                AnimateBar(PART_HealthBar, _abnormalityColors[healthAbnormality]);

            if (staminaAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_StaminaBar, _defaultStaminaBrush);
            else
                AnimateBar(PART_StaminaBar, _abnormalityColors[staminaAbnormality]);

            if (recoverableAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_RecoverableHealthBar, _defaultRecoverableBrush);
            else
                AnimateBar(PART_RecoverableHealthBar, _abnormalityColors[recoverableAbnormality]);

        });

    private void OnActiveAbnormalitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => HandleAbnormalityCategoryChange();

    public override void Dispose()
    {
        UnhookEvents();

        base.Dispose();
    }
}
