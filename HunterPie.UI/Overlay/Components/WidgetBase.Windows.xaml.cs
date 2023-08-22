using HunterPie.UI.Platform.Windows.Native;
using System.Runtime.Versioning;

namespace HunterPie.UI.Overlay.Components;

public partial class WidgetBase
{
    // TODO: Move this to platform dependent classes
    [SupportedOSPlatform("Windows")]
    private const uint Flags =
        (uint)(User32.SWP_WINDOWN_FLAGS.SWP_SHOWWINDOW
               | User32.SWP_WINDOWN_FLAGS.SWP_NOMOVE
               | User32.SWP_WINDOWN_FLAGS.SWP_NOSIZE
               | User32.SWP_WINDOWN_FLAGS.SWP_NOACTIVATE);

    [SupportedOSPlatform("Windows")]
    private const uint ClickThroughFlags =
        (uint)(User32.EX_WINDOW_STYLES.WS_EX_TRANSPARENT
               | User32.EX_WINDOW_STYLES.WS_EX_TOPMOST
               | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE
               | User32.EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);

    [SupportedOSPlatform("Windows")]
    private const uint WindowFlags =
        (uint)(User32.EX_WINDOW_STYLES.WS_EX_TOPMOST
               | User32.EX_WINDOW_STYLES.WS_EX_NOACTIVATE
               | User32.EX_WINDOW_STYLES.WS_EX_TOOLWINDOW);

}