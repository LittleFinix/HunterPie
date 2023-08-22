using Avalonia.Controls;
using App = Avalonia.Application;

namespace HunterPie.UI.Assets.Application;

public static class Resources
{
    public static ImageSource? Icon(string iconName) => App.Current?.FindResource(iconName) as ImageSource;

    public static T? Get<T>(string resourceName)
    {
        object? obj = App.Current?.FindResource(resourceName);

        if (obj is T t)
            return t;
        return default;
    }
}
