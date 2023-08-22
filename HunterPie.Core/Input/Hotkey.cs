using Avalonia.Input;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Input;

// TODO: Refactor this because this is HunterPie v1 code and I hate it
public partial class Hotkey
{
    private static readonly Dictionary<int, Action> hotkeys = new();

    private static int currId = 1;
    
    /// <summary>
    /// All hotkeys registered in HunterPie's environment
    /// </summary>
    public static IReadOnlyDictionary<int, Action> Hotkeys => hotkeys;
    
    static Hotkey()
    {
        if (OperatingSystem.IsWindows())
            LoadWindows();
        else if (OperatingSystem.IsLinux())
            LoadLinux();
        else
            throw new PlatformNotSupportedException();
    }

    internal static void Unload()
    {
        while (hotkeys.Keys.Count > 0)
        {
            _ = Unregister(hotkeys.Keys.ElementAt(0));
        }
        
        if (OperatingSystem.IsWindows())
            UnloadWindows();
        else if (OperatingSystem.IsLinux())
            UnloadLinux();
        else
            throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// Registers a new global hotkey for HunterPie.
    /// </summary>
    /// <param name="keys">String with the hotkey (e.g: "Ctrl+Alt+O")</param>
    /// <param name="callback">The function HunterPie should invoke when the hotkey is triggered</param>
    /// <returns>The hotkey id, it should be used when unregistering the hotkey</returns>
    public static int Register(KeyGesture keys, Action callback)
    {
        int result = -1;
        
        if (OperatingSystem.IsWindows())
            result = RegisterWindows(currId++, keys, callback);
        else if (OperatingSystem.IsLinux())
            result = RegisterLinux(currId++, keys, callback);
        else
            throw new PlatformNotSupportedException();
        
        if (result == -1)
            Log.Error($"Failed to register hotkey. {Marshal.GetLastPInvokeError()}: {Marshal.GetLastPInvokeErrorMessage()}");

        hotkeys[result] = callback;
        
        return result;
    }

    /// <summary>
    /// Unregisters a global hotkey from HunterPie.
    /// </summary>
    /// <param name="id">The id you got from <see cref="Register(string, Action)"/></param>
    /// <returns>True if the hotkey was unregistered successfully, false otherwise</returns>
    public static bool Unregister(int id)
    {
        if (OperatingSystem.IsWindows())
            return UnregisterWindows(id);
        if (OperatingSystem.IsLinux())
            return UnregisterLinux(id);
        throw new PlatformNotSupportedException();

    }
}
