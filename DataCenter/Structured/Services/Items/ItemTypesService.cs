using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Items;
using DBI.DataCenter.Structured.Models.Items;

namespace DBI.DataCenter.Structured.Services.Items;

public class ItemTypesService(RawItemTypesService? rawItemTypesService, RawEvolutiveItemTypesService? rawEvolutiveItemTypesService, LanguagesService? languagesService)
{
    public IEnumerable<ItemType>? GetItemTypes() => rawItemTypesService?.GetItemTypes().Select(Cook);

    public IEnumerable<ItemType>? GetItemTypesInSuperType(ItemSuperType superType) => rawItemTypesService?.GetItemTypes().Where(t => t.SuperTypeId == (int)superType).Select(Cook);

    public ItemType? GetItemType(int itemTypeId)
    {
        RawItemType? itemType = rawItemTypesService?.GetItemType(itemTypeId);
        return itemType == null ? null : Cook(itemType);
    }

    ItemType Cook(RawItemType type) =>
        new()
        {
            Id = type.Id,
            Name = languagesService?.Get(type.NameId),
            Gender = type.Gender,
            Plural = type.Plural,
            SuperType = (ItemSuperType)type.SuperTypeId,
            EvolutiveType = rawEvolutiveItemTypesService?.GetEvolutiveItemType(type.EvolutiveTypeId)?.Cook(),
            CraftXpRatio = type.CraftXpRatio,
            Mimickable = type.Mimickable,
            IsInEncyclopedia = type.IsInEncyclopedia,
            AdminSelectionTypeName = type.AdminSelectionTypeName
        };
}
