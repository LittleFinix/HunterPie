using Avalonia.Input;
using Avalonia.Threading;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading;
using X11;
using static X11.Xlib;

namespace HunterPie.Core.Input;

// TODO: Refactor this because this is HunterPie v1 code and I hate it
public partial class Hotkey
{
    private const uint InputOnly = 2;
    private const nint CopyFromParent = 0;
    
    private static IntPtr _display;
    private static Window _window;

    private static Thread _thread;

    private static object _sync = new();

    private static Dictionary<ulong, int> _x11KeyToIdMap = new();

    private static ulong HashX11Key(uint keycode, uint modifiers)
    {
        return ((ulong)modifiers << 32) | keycode;
    }
    
    [SupportedOSPlatform("Linux")]
    private static void LoadLinux()
    {
        XSetWindowAttributes attr = new() { event_mask = EventMask.KeyPressMask };

        _display = XOpenDisplay(null);
        XSynchronize(_display, true);
        _window = XDefaultRootWindow(_display);

        if (_window == Window.None)
        {
            Log.Error("Failed to setup X11 input-only window, key-bindings disabled");
            return;
        }
        
        // XGrabKey(_display, XKeysymToKeycode(_display, (KeySym)0xff14), KeyButtonMask.AnyModifier, _window, true,
        //     GrabMode.Async, GrabMode.Async);
        //
        // XGrabKey(_display, XKeysymToKeycode(_display, (KeySym)0x004f), KeyButtonMask.ControlMask | KeyButtonMask.ShiftMask, _window, true,
        //     GrabMode.Async, GrabMode.Async);

        _thread = new(RunLinuxInputThread) { IsBackground = true, Name = "X11 Hotkey Thread" };
        _thread.Start();
    }

    private static unsafe void RunLinuxInputThread()
    {
        XKeyEvent kevent = new();
        EventMask emask = EventMask.KeyPressMask;
        
        lock (_sync)
            XSelectInput(_display, _window, emask);
        
        while (_display != 0 && _window != 0)
        {
            XWindowEvent(_display, _window, emask, (IntPtr)(&kevent));
            if (kevent.type == (int)Event.KeyPress)
            {
                ulong hash = HashX11Key(kevent.keycode, kevent.state & ~(uint)KeyButtonMask.Mod2Mask);

                if (_x11KeyToIdMap.TryGetValue(hash, out int id)
                    && Hotkeys.TryGetValue(id, out var action))
                    Dispatcher.UIThread.Invoke(action);
            }
        }
    }

    [SupportedOSPlatform("Linux")]
    private static void UnloadLinux()
    {
        if (_window != 0 && _window != XDefaultRootWindow(_display))
        {
            XDestroyWindow(_display, _window);
            _window = 0;
        }

        if (_display != 0)
        {
            XCloseDisplay(_display);
            _display = 0;
        }
    }

    [SupportedOSPlatform("Linux")]
    private static int RegisterLinux(int id, KeyGesture keys, Action callback)
    {
        if (_display == 0 || _window == Window.None)
            return -1;
        
        KeyButtonMask mask = 0;

        if (keys.KeyModifiers.HasFlag(KeyModifiers.Alt))
            mask |= KeyButtonMask.Mod1Mask;
        if (keys.KeyModifiers.HasFlag(KeyModifiers.Shift))
            mask |= KeyButtonMask.ShiftMask;
        if (keys.KeyModifiers.HasFlag(KeyModifiers.Control))
            mask |= KeyButtonMask.ControlMask;
        if (keys.KeyModifiers.HasFlag(KeyModifiers.Meta))
            mask |= KeyButtonMask.Mod4Mask;

        KeySym sym = (KeySym)X11KeyTransform.VirtualKeyFromKey(keys.Key);

        if (sym == 0)
            return -1;

        KeyCode code;
        
        lock (_sync)
            code = XKeysymToKeycode(_display, sym);

        ulong hash = HashX11Key((uint)code, (uint)mask);
        if (_x11KeyToIdMap.ContainsKey(hash))
            return -1;

        lock (_sync)
        {
            XGrabKey(_display, code, mask, _window, true, GrabMode.Async, GrabMode.Async);
            XGrabKey(_display, code, mask | KeyButtonMask.Mod2Mask, _window, true, GrabMode.Async, GrabMode.Async);
        }

        return _x11KeyToIdMap[hash] = id;
    }

    [SupportedOSPlatform("Linux")]
    public static bool UnregisterLinux(int id)
    {
        return false;
        // if (!Hotkeys.ContainsKey(id))
        // {
        //     Log.Info($"Failed to unregister hotkey with id: {id}. Hotkey not found!");
        //     return false;
        // }
        //
        // bool success = KeyboardHookHelper.UnregisterHotKey(hWnd, id);
        //
        // if (success)
        // {
        //     _ = hotkeys.Remove(id);
        //     return true;
        // }
        // else
        // {
        //     Log.Error($"Failed to unregister hotkey. {Marshal.GetLastWin32Error()}");
        //     _ = hotkeys.Remove(id);
        //     return true;
        // }
    }
}
