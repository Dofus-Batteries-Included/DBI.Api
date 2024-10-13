using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Structured.Models.Effects;

public class EffectInstanceMount : EffectInstance
{
    public EffectInstanceMount() { }

    internal EffectInstanceMount(RawEffectInstanceMount instance)
    {
        Id = instance.Id;
        ExpirationDate = instance.ExpirationDate;
        Model = instance.Model;
        Name = instance.Name;
        Owner = instance.Owner;
        Level = instance.Level;
        Sex = instance.Sex;
        IsRideable = instance.IsRideable;
        IsFeconded = instance.IsFeconded;
        IsFecondationReady = instance.IsFecondationReady;
        ReproductionCount = instance.ReproductionCount;
        ReproductionCountMax = instance.ReproductionCountMax;
        Effects = instance.Effects.Select(e => e.Cook()).ToArray();
        Capacities = instance.Capacities;
    }

    public long Id { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Model { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public int Level { get; set; }
    public bool Sex { get; set; }
    public bool IsRideable { get; set; }
    public bool IsFeconded { get; set; }
    public bool IsFecondationReady { get; set; }
    public int ReproductionCount { get; set; }
    public int ReproductionCountMax { get; set; }
    public IReadOnlyList<EffectInstance> Effects { get; set; } = [];
    public IReadOnlyList<int> Capacities { get; set; } = [];
}
