using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Localization;

namespace HunterPie.UI.Architecture.Adapter;
public class MonsterNameAdapter
{
    public static string From(GameType game, int monsterId)
    {
        return Localization.FindString("Monsters", $"{game}", "Monster", $"{monsterId}");
    }
}
