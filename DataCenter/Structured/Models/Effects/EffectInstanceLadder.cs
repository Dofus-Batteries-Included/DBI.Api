using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceLadder : EffectInstanceCreature
{
    public EffectInstanceLadder() { }

    internal EffectInstanceLadder(RawEffectInstanceLadder instance)
    {
        MonsterCount = instance.MonsterCount;
    }

    public int MonsterCount { get; set; }
}
