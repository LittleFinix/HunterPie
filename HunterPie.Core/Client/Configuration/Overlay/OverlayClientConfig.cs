using Avalonia.Input;
using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration.Overlay;

[SettingsGroup("OVERLAY_STRING", "ICON_OVERLAY")]
public class OverlayClientConfig : ISettings
{
    [SettingField("OVERLAY_ENABLED_STRING")]
    public Observable<bool> IsEnabled { get; set; } = true;

    [SettingField("OVERLAY_KEYBINDING_TOGGLE_VISIBILITY_STRING")]
    public KeyGesture ToggleVisibility { get; set; } = KeyGesture.Parse("Ctrl+Alt+V");

    [SettingField("OVERLAY_KEYBINDING_TOGGLE_DESIGN_MODE", requiresRestart: true)]
    public KeyGesture ToggleDesignMode { get; set; } = KeyGesture.Parse("Scroll");

    [SettingField("OVERLAY_HIDE_WHEN_GAME_UNFOCUS_STRING")]
    public Observable<bool> HideWhenUnfocus { get; set; } = false;
}
