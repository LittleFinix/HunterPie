using HunterPie.Core.System.Linux;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;

namespace HunterPie.Features.Backups;

internal partial class GameSaveBackupService
{
    [SupportedOSPlatform("Linux")]
    private bool TryGetSteamProcessLinux([MaybeNullWhen(false)] out int activeUser, [MaybeNullWhen(false)] out string? steamClientPath)
    {
        // excerpt from /usr/games/steam:
        // > According to Valve, ~/.steam is intended to be a control directory containing
        // > symbolic links pointing to the currently-running or most-recently-run Steam
        // > installation. This is part of Steam's API, and is relied on by external
        // > components.
        // 
        // further it says
        // > ~/.steam/steam is a symlink to the Steam data directory ...
        //
        // While this is a secondary source, this seems accurate, therefore we simply have to read the corresponding link.
        //
        
        steamClientPath = Path.Join(Environment.GetEnvironmentVariable("HOME"), ".steam", "steam");
        if (!int.TryParse(Environment.GetEnvironmentVariable("UID"), out activeUser))
            return false;

        // fully resolve symlink
        while (Posix.TryReadLink(steamClientPath, out steamClientPath))
            ;
        
        return true;
    }
}
