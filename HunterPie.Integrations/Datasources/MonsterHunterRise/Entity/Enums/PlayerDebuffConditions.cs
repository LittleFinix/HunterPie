﻿namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

[Flags]
public enum DebuffConditions : ulong
{
    None = 0,
    NoData1 = 1,
    FireBlight = 1 << 1,
    NoData2 = 1 << 2,
    WaterBlight = 1 << 3,
    NoData3 = 1 << 4,
    ThunderBlight = 1 << 5,
    NoData4 = 1 << 6,
    IceBlight = 1 << 7,
    NoData5 = 1 << 8,
    DragonBlight = 1 << 9,
    AllResDownS = 1 << 10,
    AllResDownL = 1 << 11,
    VitalDamaging = 1 << 12,
    BubbleBlightS = 1 << 13,
    RedBubbleS = 1 << 14,
    GreenBubbleS = 1 << 15,
    FrenzyLatency = 1 << 16,
    FrenzyOnsetNearness = 1 << 17,
    FrenzyOvercomeNearness = 1 << 18,
    FrenzyOnset = 1 << 19,
    FrenzyOvercome = 1 << 20,
    Territory = 1 << 21,
    HellfireBlight = 1 << 22,
    HellfireBlastNearness = 1 << 23,
    BlastBlight = 1 << 24,
    BlastNearness = 1 << 25,
    Blast = 1 << 26,
    GoldMudSlip = 1 << 27,
    MagmaSlip = 1 << 28,
    EnemyFireSlip = 1 << 29,
    NoData6 = 1 << 30,
    NoData7 = 1UL << 31,
    Poison = 1UL << 32,
    NoxiousPoison = 1UL << 33,
    DeadlyPoison = 1UL << 34,
    Sleep = 1UL << 35,
    Paralyze = 1UL << 36,
    Stun = 1UL << 37,
    Stench = 1UL << 38,
    StinkMink = 1UL << 39,
    StinkMinkAfter = 1UL << 40,
    BubbleBlightL = 1UL << 41,
    RedBubbleL = 1UL << 42,
    GreenBubbleL = 1UL << 43,
    Leeched = 1UL << 44,
    Bleed = 1UL << 45,
    Confusion = 1UL << 46,
    DefenceDownS = 1UL << 47,
    DefenceDownL = 1UL << 48,
    Webbed = 1UL << 49,
    VirusLatencyMiddle = 1UL << 51,
    BloodBlight = 1UL << 52,
    BloodBlightHeal = 1UL << 53
}