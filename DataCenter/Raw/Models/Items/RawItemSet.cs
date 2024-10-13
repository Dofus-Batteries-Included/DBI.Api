using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Raw.Models.Items;

public class RawItemSet
{
    public int Id { get; set; }
    public int NameId { get; set; }
    public IReadOnlyList<uint> Items { get; set; }
    public bool BonusIsSecret { get; set; }
    public IReadOnlyList<IReadOnlyList<RawEffectInstance>> Effects { get; set; } = [];
}
