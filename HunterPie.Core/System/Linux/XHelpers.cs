using Avalonia.Controls.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using X11;

namespace HunterPie.Core.System.Linux;

[SupportedOSPlatform("Linux")]
public static class XHelpers
{
    public const string CLIENT_LIST = "_NET_CLIENT_LIST";
    public const string ACTIVE_WINDOW = "_NET_ACTIVE_WINDOW";
    public const string WM_PID = "_NET_WM_PID";

    private static IntPtr _display;
    
    public static IntPtr GetDisplay()
    {
        if (_display == 0)
            _display = Xlib.XOpenDisplay(null);

        return _display;
    }
    
    [DllImport("libX11.so.6", CharSet = CharSet.Ansi)]
    private static extern Atom XInternAtom(IntPtr display, string name, bool onlyIfExists);
    
    [DllImport("libX11.so.6", CharSet = CharSet.Ansi)]
    private static extern unsafe int XFree(void *data);
    
    [DllImport("libX11.so.6", CharSet = CharSet.Ansi)]
    private static extern unsafe int XGetWindowProperty([In] IntPtr display, Window w, Atom property,
        long longOffset, long longLength,
        bool delete,
        Atom reqType,
        out Atom actualTypeReturn,
        out int actualFormatReturn,
        out ulong nItemsReturn,
        out ulong bytesAfterReturn,
        [Out] out byte *propReturn);

    public static unsafe Property GetWindowProperty(IntPtr display, Window w, Atom type, string name, int count)
    {
        var atom = XInternAtom(display, name, true);

        XGetWindowProperty(display, w,
            atom, 0, count,
            false, type, out _,
            out int actualFormat, out ulong items, out ulong bytesLeft, out byte* props);
            
        // if (bytesLeft != 0)
        //     throw new Exception();

        return new(props, items, actualFormat);
    }

    public static unsafe T GetWindowProperty<T>(IntPtr display, Window w, Atom type, string name) where T : unmanaged
    {
        using var props = GetWindowProperty(display, w, type, name, 1);
        return props.Count == 0 ? default : props.OfType<T>().ToArray()[0];
    }

    public static Window GetActiveWindow(IntPtr display)
    {
        return GetWindowProperty<Window>(display, Xlib.XDefaultRootWindow(display), Atom.Window, ACTIVE_WINDOW);
    }
    
    public static IEnumerable<Window> EnumerateWindows(IntPtr display)
    {
        using var props = GetWindowProperty(display, Xlib.XDefaultRootWindow(display), Atom.Window, CLIENT_LIST, int.MaxValue);
        return props.OfType<Window>().ToArray();
    }

    public static Window FindWindowForProcess(IntPtr display, int pid)
    {
        foreach (var win in EnumerateWindows(display))
        {
            int prop = GetWindowProperty<int>(display, win, Atom.Cardinal, WM_PID);
            if (prop == pid)
                return win;
        }

        return Window.None;
    }

    public class Property : IDisposable
    {
        private readonly unsafe byte* _bytes;
        private readonly ulong _count;
        private readonly uint _width;

        public unsafe Property(byte* bytes, ulong count, int width)
        {
            _bytes = bytes;
            _count = count;
            _width = checked((uint)width);

            if ((_count * _width) > int.MaxValue)
                throw new ArgumentException();
        }

        public int Count => checked((int)_count);
        public int Width => checked((int)_width);

        public unsafe Span<byte> Bytes => new Span<byte>(_bytes, checked((int)(_count * _width)));

        public unsafe Span<T> OfType<T>() where T : unmanaged
        {
            if (_count == 0)
                return Span<T>.Empty;
            
            // var type = typeof(T);
            //
            // if (type.IsEnum)
            //     type = Enum.GetUnderlyingType(type);
            //
            // if (Marshal.SizeOf(type) * 8 != _width)
            //     throw new Exception();

            return MemoryMarshal.Cast<byte, T>(Bytes);
        }
        
        private unsafe void ReleaseUnmanagedResources()
        {
            XFree(_bytes);
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Property()
        {
            ReleaseUnmanagedResources();
        }
    }
}