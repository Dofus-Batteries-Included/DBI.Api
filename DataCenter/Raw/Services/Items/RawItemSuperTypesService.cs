using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemSuperTypesService(IReadOnlyCollection<RawItemSuperType> itemSuperTypes)
{
    readonly Dictionary<int, RawItemSuperType> _itemSuperTypes = itemSuperTypes.ToDictionary(itemSuperType => itemSuperType.Id, itemSuperType => itemSuperType);

    public RawItemSuperType? GetItemSuperType(ushort itemSuperTypeId) => _itemSuperTypes.GetValueOrDefault(itemSuperTypeId);
    public IEnumerable<RawItemSuperType> GetItemSuperTypes() => _itemSuperTypes.Values;
}
