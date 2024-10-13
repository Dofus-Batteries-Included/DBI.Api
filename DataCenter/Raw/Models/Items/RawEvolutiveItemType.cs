namespace DBI.DataCenter.Raw.Models.Items;

public class RawEvolutiveItemType
{
    public int Id { get; set; }
    public int MaxLevel { get; set; }
    public double ExperienceBoost { get; set; }
    public IReadOnlyList<int> ExperienceByLevel { get; set; } = [];
}
