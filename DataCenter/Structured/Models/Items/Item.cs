using DBI.DataCenter.Structured.Models.Effects;
using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Structured.Models.Items;

public class Item
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
    ///     The possible effects of the item.
    /// </summary>
    public IReadOnlyList<EffectInstance> PossibleEffects { get; set; } = [];

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

    /// <summary>
    ///     Is the item two-handed?
    /// </summary>
    public bool TwoHanded { get; set; }

    /// <summary>
    ///     Is the item usable?
    /// </summary>
    public bool Usable { get; set; }

    /// <summary>
    ///     Does the item needs user confirmation before use?
    /// </summary>
    public bool NeedUseConfirm { get; set; }

    /// <summary>
    ///     Is using the item on another character forbidden?
    /// </summary>
    public bool NonUsableOnAnother { get; set; }

    /// <summary>
    ///     Is the item useable by targeting another character or a cell?
    /// </summary>
    public bool Targetable { get; set; }

    /// <summary>
    ///     Is the item exchangeable?
    /// </summary>
    public bool Exchangeable { get; set; }

    /// <summary>
    ///     Is the item enhanceable?
    /// </summary>
    public bool Enhanceable { get; set; }

    /// <summary>
    ///     Is the item ethereal?
    /// </summary>
    public bool Ethereal { get; set; }

    /// <summary>
    ///     Is the item cursed?
    /// </summary>
    public bool Cursed { get; set; }

    /// <summary>
    ///     Is the item destructible by the user?
    /// </summary>
    public bool IsDestructible { get; set; }

    /// <summary>
    ///     Is the item legendary?
    /// </summary>
    public bool IsLegendary { get; set; }

    /// <summary>
    ///     Is the item colorable?
    /// </summary>
    public bool IsColorable { get; set; }

    /// <summary>
    ///     Is the item sealable?
    /// </summary>
    public bool IsSealable { get; set; }

    /// <summary>
    ///     Are the effects of the item hidden?
    /// </summary>
    public bool HideEffects { get; set; }

    /// <summary>
    ///     Is the bonus of the item secret?
    /// </summary>
    public bool BonusIsSecret { get; set; }

    /// <summary>
    ///     Is the item displayable on the official website?
    /// </summary>
    public bool ObjectIsDisplayOnWeb { get; set; }

    /// <summary>
    ///     The animation that the character plays when they use the item.
    /// </summary>
    public sbyte UseAnimationId { get; set; }

    /// <summary>
    ///     The visibility condition.
    /// </summary>
    public string Visibility { get; set; } = "";

    /// <summary>
    ///     The use criteria.
    /// </summary>
    public string Criteria { get; set; } = "";

    /// <summary>
    ///     The target of the use criteria.
    /// </summary>
    public string CriteriaTarget { get; set; } = "";

    /// <summary>
    ///     The appearance ID.
    /// </summary>
    public ushort AppearanceId { get; set; }

    /// <summary>
    ///     The important notice, if any.
    /// </summary>
    public string ImportantNotice { get; set; } = "";

    /// <summary>
    ///     The version of change.
    /// </summary>
    public string ChangeVersion { get; set; } = "";

    /// <summary>
    ///     The tooltip expiration date.
    /// </summary>
    public double TooltipExpirationDate { get; set; }
}
