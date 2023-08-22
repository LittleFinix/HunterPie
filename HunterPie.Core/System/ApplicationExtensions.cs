using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace HunterPie.UI;

public static class ApplicationExtensions
{
    public static Window? GetMainWindow(this Application app)
    {
        if (app.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            return lifetime.MainWindow;
        return null;
    }

    private static Screens? _screens;
    
    public static Screens? GetScreens(this Application app)
    {
        if (_screens is not null)
            return _screens;
        
        if (app.GetMainWindow() is WindowBase window)
            return _screens = window.Screens;
        var w = new Window();
        return _screens = w.Screens;
    }
}