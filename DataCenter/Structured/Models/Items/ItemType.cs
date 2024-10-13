using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Structured.Models.Items;

/// <summary>
///     A type of items.
/// </summary>
public class ItemType
{
    /// <summary>
    ///     The unique ID of the item type.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The name of the item type.
    /// </summary>
    public LocalizedText? Name { get; set; }

    /// <summary>
    ///     The gender of the name.
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    ///     Is the name plural.
    /// </summary>
    public bool Plural { get; set; }

    /// <summary>
    ///     The super type.
    /// </summary>
    public ItemSuperType SuperType { get; set; }

    /// <summary>
    ///     The evolutive item type.
    /// </summary>
    public EvolutiveItemType? EvolutiveType { get; set; }

    /// <summary>
    ///     The craft XP ratio.
    /// </summary>
    public int CraftXpRatio { get; set; }

    /// <summary>
    ///     Are the items of the type mimickable.
    /// </summary>
    public bool Mimickable { get; set; }

    /// <summary>
    ///     Are the items of the type in the public encyclopedia.
    /// </summary>
    public bool IsInEncyclopedia { get; set; }

    /// <summary>
    ///     The admin selection type name.
    /// </summary>
    public string AdminSelectionTypeName { get; set; } = "";
}
