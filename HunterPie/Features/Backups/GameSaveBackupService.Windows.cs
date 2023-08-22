using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace HunterPie.Features.Backups;

internal partial class GameSaveBackupService
{
    [SupportedOSPlatform("Windows")]
    private bool TryGetSteamProcessWindows(out int activeUser, [MaybeNullWhen(false)] out string? steamClientPath)
    {
        using RegistryKey? activeProcess = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam\ActiveProcess");

        activeUser = 0;
        steamClientPath = null;

        if (activeProcess is null)
            return false;

        activeUser = (int?)activeProcess.GetValue("ActiveUser") ?? 0;
        steamClientPath = (string?)activeProcess.GetValue("SteamClientDll");

        return true;
    }
}
