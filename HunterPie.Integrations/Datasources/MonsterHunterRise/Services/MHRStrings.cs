using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Services;

public class MHRStrings
{

    private readonly IProcessManager _process;

    public MHRStrings(IProcessManager process)
    {
        _process = process;

    }

    public string GetMonsterNameById(int id)
    {
        return Localization.FindString("Monsters", "Rise", "Monster", $"{id}");
    }

    public string GetStageNameById(int id)
    {
        return Localization.FindString("Monsters", "Rise", "Monster", $"{id}");
    }
}
