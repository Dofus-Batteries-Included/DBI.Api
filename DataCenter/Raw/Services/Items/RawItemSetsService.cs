using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemSetsService(IReadOnlyCollection<RawItemSet> itemSets)
{
    readonly Dictionary<int, RawItemSet> _itemSets = itemSets.ToDictionary(itemSet => itemSet.Id, itemSet => itemSet);

    public RawItemSet? GetItemSet(ushort itemSetId) => _itemSets.GetValueOrDefault(itemSetId);
    public IEnumerable<RawItemSet> GetItemSets() => _itemSets.Values;
}
