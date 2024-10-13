using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Structured.Models.Items;

public class EvolutiveItemType
{
    /// <summary>
    ///     The unique ID of the evolutive item type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The max level of the evolutive items of the type.
    /// </summary>
    public int MaxLevel { get; set; }

    /// <summary>
    ///     The experience boost of the evolutive items of the type.
    /// </summary>
    public double ExperienceBoost { get; set; }

    /// <summary>
    ///     The experience required for each level of the evolutive items of the type.
    /// </summary>
    public IReadOnlyList<int> ExperienceByLevel { get; set; } = [];
}

static class EvolutiveItemTypeMappingExtensions
{
    public static EvolutiveItemType Cook(this RawEvolutiveItemType type) =>
        new()
        {
            Id = type.Id,
            MaxLevel = type.MaxLevel,
            ExperienceBoost = type.ExperienceBoost,
            ExperienceByLevel = type.ExperienceByLevel
        };
}
