using Avalonia;
using HunterPie.Core.System.Linux;
using HunterPie.Core.System.Windows;
using HunterPie.UI;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Common;

public static partial class ProcessWindowExtensions
{
    [SupportedOSPlatform("Linux")]
    private static ISimpleWindowInfo? GetMainWindowLinux(this Process process)
    {
        return new LinuxWindowInfo(process);
    }
}
