namespace DBI.DataCenter.Raw.Models.Effects;

public class RawEffectInstanceMount : RawEffectInstance
{
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
    public IReadOnlyList<RawEffectInstance> Effects { get; set; } = [];
    public IReadOnlyList<int> Capacities { get; set; } = [];
}
