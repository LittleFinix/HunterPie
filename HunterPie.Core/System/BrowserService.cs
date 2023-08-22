using HunterPie.Core.System.Common;

namespace HunterPie.Core.System;
public static class BrowserService
{
    public static void OpenUrl(string url) =>
        Launcher.Open(url);

    public static void OpenFolder(string path) =>
        Launcher.Open(path);
}
