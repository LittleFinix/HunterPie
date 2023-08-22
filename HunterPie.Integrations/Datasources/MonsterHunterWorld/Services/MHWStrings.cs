using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Services;

public class MHWStrings
{
    private readonly IProcessManager _process;

    public MHWStrings(IProcessManager process)
    {
        _process = process;
    }

    public string GetMonsterNameById(int id)
    {
        return Localization.FindString("Monsters", "World", "Monster", $"{id}");
    }

    public string GetStageNameById(int id)
    {
        return Localization.FindString("Stages", "World", "Stage", $"{id}");
    }
}
