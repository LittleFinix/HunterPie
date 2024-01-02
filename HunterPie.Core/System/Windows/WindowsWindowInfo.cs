using Avalonia;
using HunterPie.Core.System.Common;
using HunterPie.Core.System.Common.Exceptions;
using HunterPie.Core.System.Windows.Native;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Windows;

[SupportedOSPlatform("Windows")]
public class WindowsWindowInfo : ISimpleWindowInfo
{
    private readonly Process _process;

    public WindowsWindowInfo(Process process)
    {
        _process = process;
    }

    public IntPtr Handle => _process.MainWindowHandle;

    public bool Valid => Handle != 0;
    public bool Open => true;
    
    public bool IsFocused => User32.GetForegroundWindow() == Handle;
    public string Title => _process.MainWindowTitle;

    public PixelRect ClientArea
    {
        get
        {
                if (!User32.GetClientRect(Handle, out var rect))
                    throw new NativeErrorException();

            return new(
                (int)rect.Left,
                (int)rect.Top,
                (int)(rect.Right - rect.Left),
                (int)(rect.Bottom - rect.Top)
            );
        }
    }
}