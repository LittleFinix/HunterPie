using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Client.Configuration.Versions;

public class VersionedConfig : IVersionedConfig, IAbstractHunterPieConfig
{
    public int Version { get; }

    public VersionedConfig(int version)
    {
        Version = version;
    }
}
