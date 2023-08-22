using Avalonia.Input;
using HunterPie.Core.Logger;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace HunterPie.Core.Input;

// TODO: Refactor this because this is HunterPie v1 code and I hate it
public partial class Hotkey
{
    private const int WM_HOTKEY = 0x0312;

    private static IntPtr hWnd;

    [SupportedOSPlatform("Windows")]
    private static void LoadWindows() {}
    
    [SupportedOSPlatform("Windows")]
    private static void UnloadWindows()
    {
    }

#pragma warning disable IDE0060
    [SupportedOSPlatform("Windows")]
    internal static IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
#pragma warning restore IDE0060
    {
        if (msg == WM_HOTKEY)
        {
            int hotkeyId = wParam.ToInt32();
            if (Hotkeys.ContainsKey(hotkeyId))
            {
                Action callback = Hotkeys[hotkeyId];
                callback.Invoke();
            }
        }

        return IntPtr.Zero;
    }

    [SupportedOSPlatform("Windows")]
    private static int RegisterWindows(int id, KeyGesture keys, Action callback)
    {
        int modifier = 0x4000;

        if (keys.KeyModifiers.HasFlag(KeyModifiers.Alt))
            modifier |= 0x0001;
        
        if (keys.KeyModifiers.HasFlag(KeyModifiers.Control))
            modifier |= 0x0002;
        
        if (keys.KeyModifiers.HasFlag(KeyModifiers.Shift))
            modifier |= 0x0004;

        int key = Avalonia.Win32.Input.KeyInterop.VirtualKeyFromKey(keys.Key);
    
        // Skip hotkeys which value is "None"
        if (key == 0)
        {
            return 0;
        }
    
        bool success = KeyboardHookHelper.RegisterHotKey(hWnd, id, modifier, key);
    
        if (success)
        {
            hotkeys[id] = callback;
            return id;
        }
        else
        {
            return -1;
        }
    }

    [SupportedOSPlatform("Windows")]
    public static bool UnregisterWindows(int id)
    {
        if (!Hotkeys.ContainsKey(id))
        {
            Log.Info($"Failed to unregister hotkey with id: {id}. Hotkey not found!");
            return false;
        }
        
        bool success = KeyboardHookHelper.UnregisterHotKey(hWnd, id);
        
        if (success)
        {
            _ = hotkeys.Remove(id);
            return true;
        }
        else
        {
            Log.Error($"Failed to unregister hotkey. {Marshal.GetLastWin32Error()}");
            _ = hotkeys.Remove(id);
            return true;
        }
    }
}
