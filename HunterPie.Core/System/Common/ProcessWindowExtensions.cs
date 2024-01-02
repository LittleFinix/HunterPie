using Avalonia;
using HunterPie.UI;
using System;
using System.Diagnostics;

namespace HunterPie.Core.System.Common;

public static partial class ProcessWindowExtensions
{
    public static ISimpleWindowInfo? GetMainWindow(this Process process)
    {
        if (OperatingSystem.IsWindows())
            return process.GetMainWindowWindows();
        else if (OperatingSystem.IsLinux())
            return process.GetMainWindowLinux();
        else
            throw new PlatformNotSupportedException();
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
    public bool Valid { get; }
    public bool Open { get; }
    
    public bool IsFocused { get; }
    public string Title { get; }
    public PixelRect ClientArea { get; }
}