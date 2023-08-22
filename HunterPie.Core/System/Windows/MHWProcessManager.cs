using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Common;
using System;
using System.Diagnostics;
using System.IO;

namespace HunterPie.Core.System.Windows;

internal class MHWProcessManager : ProcessManagerBase
{
    public override string Name => "MonsterHunterWorld";
    public override GameProcess Game => GameProcess.MonsterHunterWorld;

    protected override bool ShouldOpenProcess(Process process)
    {
        // If our process is in either another window, or not initialized yet
        
        if (OperatingSystem.IsWindows() && !process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER: WORLD"))
            return false;

        string version = OperatingSystem.IsWindows() ? process.MainWindowTitle.Split('(')[1].Trim(')') : string.Empty;
        bool parsed = int.TryParse(version, out int parsedVersion);

        if (!parsed)
        {
            Log.Error("Failed to get Monster Hunter: World build version. Loading latest map version instead.");
            _ = AddressMap.ParseLatest(ClientInfo.AddressPath);
        }
        else
        {
            _ = IsICE(parsedVersion)
                ? AddressMap.ParseLatest(ClientInfo.AddressPath)
                : AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterWorld.{version}.map"));
        }

        if (!AddressMap.IsLoaded)
        {
            Log.Error("Failed to parse map file");
            return false;
        }

        return true;
    }

    private bool IsICE(int version) => version is >= 300_000 and <= 400_000;
}
