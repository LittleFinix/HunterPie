using System.Runtime.CompilerServices;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[InlineArray(3)]
public struct KinsectBuffTypeArray
{
    KinsectBuffType Element;
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct MHWInsectGlaiveStructure
{
    /// <summary>
    /// Pointer to <see cref="MHWKinsectStructure"/>
    /// </summary>
    [FieldOffset(0x2350)] public long KinsectPointer;
    [FieldOffset(0x2368)] public float AttackTimer;
    [FieldOffset(0x236C)] public float SpeedTimer;
    [FieldOffset(0x2370)] public float DefenseTimer;
    [FieldOffset(0x2378)] public KinsectBuffTypeArray BuffsQueue;
    [FieldOffset(0x2388)] public int QueuedIndex;
    [FieldOffset(0x2390)] public int QueuedBuffsCount;
}