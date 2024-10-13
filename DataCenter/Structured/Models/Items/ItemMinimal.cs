using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Structured.Models.Items;

public class ItemMinimal
{
    /// <summary>
    ///     The super type of the item.
    /// </summary>
    public ItemSuperType? SuperType { get; set; }

    /// <summary>
    ///     The unique ID of the type of the item.
    /// </summary>
    public int ItemTypeId { get; set; }

    /// <summary>
    ///     The name of the type of the item.
    /// </summary>
    public LocalizedText? ItemTypeName { get; set; }

    /// <summary>
    ///     The unique ID of the item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     The level of the item.
    /// </summary>
    public byte Level { get; set; }

    /// <summary>
    ///     The ID of the icon of the item.
    /// </summary>
    public int IconId { get; set; }

    /// <summary>
    ///     The name of item.
    /// </summary>
    public LocalizedText? Name { get; set; }

    /// <summary>
    ///     The description of the item.
    /// </summary>
    public LocalizedText? Description { get; set; }

    /// <summary>
    ///     The base price of the item.
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    ///     The weight of the item.
    /// </summary>
    public uint Weight { get; set; }

    /// <summary>
    ///     The item set containing the item, if any.
    /// </summary>
    public int? ItemSetId { get; set; }

    /// <summary>
    ///     The name of the item set containing the item, if any.
    /// </summary>
    public LocalizedText? ItemSetName { get; set; }
}
