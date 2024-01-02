using Avalonia;
using HunterPie.Core.System.Common;
using System.Diagnostics;
using System.Runtime.Versioning;
using X11;

namespace HunterPie.Core.System.Linux;

[SupportedOSPlatform("Linux")]
public class LinuxWindowInfo : ISimpleWindowInfo
{
    private readonly Process _process;

    private Window _window;

    public LinuxWindowInfo(Process process)
    {
        _process = process;
    }

    public Window Handle
    {
        get
        {
            if (_window == Window.None)
                _window = XHelpers.FindWindowForProcess(XHelpers.GetDisplay(), _process.Id);

            return _window;
        }
    }

    public XWindowAttributes Attributes =>
        Xlib.XGetWindowAttributes(XHelpers.GetDisplay(), Handle, out var attr) != Status.Failure ? attr : default;

    public bool Valid => Handle != Window.None;

    public bool Open => Attributes.map_state == 2;

    public bool IsFocused => XHelpers.GetActiveWindow(XHelpers.GetDisplay()) == Handle;

    public string Title
    {
        get
        {
            string name = string.Empty;
            Xlib.XFetchName(XHelpers.GetDisplay(), Handle, ref name);

            return name;
        }
    }

    public PixelRect ClientArea
    {
        get
        {
            var attr = Attributes;
            return new(attr.x, attr.y, (int)attr.width, (int)attr.height);
        }
    }
}