namespace HunterPie.Core.Settings;

public static class KeyGestureHelper
{
    public static string Migrate(string key)
    {
        string[] keys = key.Split('+');

        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = keys[i] switch
            {
                "ScrollLock" => "Scroll",
                { } s => s 
            };
        }

        return string.Join('+', keys);
    }
}