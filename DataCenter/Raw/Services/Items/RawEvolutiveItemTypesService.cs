using DBI.DataCenter.Raw.Models.Items;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawEvolutiveItemTypesService(IReadOnlyCollection<RawEvolutiveItemType> evolutiveItemTypes)
{
    readonly Dictionary<int, RawEvolutiveItemType> _evolutiveItemTypes = evolutiveItemTypes.ToDictionary(
        evolutiveItemType => evolutiveItemType.Id,
        evolutiveItemType => evolutiveItemType
    );

    public RawEvolutiveItemType? GetEvolutiveItemType(int evolutiveItemTypeId) => _evolutiveItemTypes.GetValueOrDefault(evolutiveItemTypeId);
    public IEnumerable<RawEvolutiveItemType> GetEvolutiveItemTypes() => _evolutiveItemTypes.Values;
}
