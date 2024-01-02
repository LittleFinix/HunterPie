using Avalonia;
using HunterPie.Core.System.Windows;
using HunterPie.UI;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace HunterPie.Core.System.Common;

public static partial class ProcessWindowExtensions
{
    [SupportedOSPlatform("Windows")]
    private static ISimpleWindowInfo? GetMainWindowWindows(this Process process)
    {
        return new WindowsWindowInfo(process);
    }
}
