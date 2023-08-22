using System.Diagnostics;

namespace HunterPie.Core.System.Common;

public class Launcher
{
    public static void Open(string fileOrUrl)
    {
        ProcessStartInfo startInfo = new()
        {
            UseShellExecute = true,
            CreateNoWindow = false,
            FileName = fileOrUrl,
        };

        _ = Process.Start(startInfo);
    }
}