using System;

namespace HunterPie.Core.Game.Enums;

[Flags]
public enum PartType
{
    Invalid = 0,
    Flinch = 1 << 1,
    Breakable = 1 << 2,
    Severable = 1 << 3,
    Qurio = 1 << 4,
}
