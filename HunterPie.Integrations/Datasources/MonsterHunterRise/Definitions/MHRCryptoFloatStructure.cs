using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct MHRCryptoFloatStructure
{
    public uint Key;
    public uint Index;

    public fixed uint Values[4];

    public uint GetValue()
    {
        return Values[Index & 3];
    }
}
