using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceCreature : EffectInstance
{
    public EffectInstanceCreature() { }

    internal EffectInstanceCreature(RawEffectInstanceCreature instance)
    {
        MonsterFamilyId = instance.MonsterFamilyId;
    }

    public short MonsterFamilyId { get; set; }
}
