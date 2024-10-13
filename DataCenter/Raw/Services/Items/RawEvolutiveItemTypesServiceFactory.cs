using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawEvolutiveItemTypesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawEvolutiveItemTypesService, RawEvolutiveItemType[]>(rawDataRepository, RawDataType.EvolutiveItemTypes, rawDataJsonOptionsProvider)
{
    protected override RawEvolutiveItemTypesService? CreateServiceImpl(RawEvolutiveItemType[] data, CancellationToken cancellationToken) => new(data);
}
