using Avalonia;
using HunterPie.UI;
using System.Diagnostics;

namespace HunterPie.Core.System.Common;

public static class ProcessWindowExtensions
{
    public static ISimpleWindowInfo? GetMainWindow(this Process process)
    {
        return null;
        // throw new PlatformNotSupportedException();
    }

    public static PixelRect GetWindowClientArea(this Process? process)
    {
        return process?.GetMainWindow()?.ClientArea ??
               Application.Current?.GetScreens()?.Primary?.WorkingArea ?? default;
    }
}

public interface ISimpleWindowInfo
{
    public string Title { get; }
    public PixelRect ClientArea { get; }
}