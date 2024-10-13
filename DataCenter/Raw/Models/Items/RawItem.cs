using DBI.DataCenter.Raw.Models.Effects;

namespace DBI.DataCenter.Raw.Models.Items;

public class RawItem
{
    public ushort Id { get; set; }
    public byte Level { get; set; }
    public int IconId { get; set; }
    public int NameId { get; set; }
    public int DescriptionId { get; set; }
    public int ItemTypeId { get; set; }
    public IReadOnlyList<RawEffectInstance> PossibleEffects { get; set; } = [];
    public IReadOnlyList<ushort> EvolutiveEffectIds { get; set; } = [];
    public float Price { get; set; }
    public uint Weight { get; set; }
    public uint RealWeight { get; set; }
    public float RecyclingNuggets { get; set; }
    public IReadOnlyList<int> FavoriteRecyclingSubAreas { get; set; } = [];
    public IReadOnlyList<IReadOnlyList<int>> ResourcesBySubarea { get; set; } = [];
    public IReadOnlyList<ushort> FavoriteSubAreas { get; set; } = [];
    public ushort FavoriteSubAreaBonus { get; set; }
    public IReadOnlyList<ushort> RecipeIds { get; set; } = [];
    public byte RecipeSlots { get; set; }
    public bool SecretRecipe { get; set; }
    public short CraftXpRatio { get; set; }
    public string CraftVisible { get; set; } = "";
    public string CraftFeasible { get; set; } = "";
    public string CraftConditional { get; set; } = "";
    public IReadOnlyList<ushort> DropMonsterIds { get; set; } = [];
    public IReadOnlyList<ushort> DropTemporisMonsterIds { get; set; } = [];
    public short ItemSetId { get; set; }
    public bool TwoHanded { get; set; }
    public bool Usable { get; set; }
    public bool NeedUseConfirm { get; set; }
    public bool NonUsableOnAnother { get; set; }
    public bool Targetable { get; set; }
    public bool Exchangeable { get; set; }
    public bool Enhanceable { get; set; }
    public bool Ethereal { get; set; }
    public bool Cursed { get; set; }
    public bool IsDestructible { get; set; }
    public bool IsLegendary { get; set; }
    public bool IsColorable { get; set; }
    public bool IsSealable { get; set; }
    public bool HideEffects { get; set; }
    public bool BonusIsSecret { get; set; }
    public bool ObjectIsDisplayOnWeb { get; set; }
    public sbyte UseAnimationId { get; set; }
    public string Visibility { get; set; } = "";
    public string Criteria { get; set; } = "";
    public string CriteriaTarget { get; set; } = "";
    public ushort AppearanceId { get; set; }
    public string ImportantNoticeId { get; set; } = "";
    public string ChangeVersion { get; set; } = "";
    public double TooltipExpirationDate { get; set; }
}
