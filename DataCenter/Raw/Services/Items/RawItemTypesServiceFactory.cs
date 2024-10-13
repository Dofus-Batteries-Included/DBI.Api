using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemTypesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawItemTypesService, RawItemType[]>(rawDataRepository, RawDataType.ItemTypes, rawDataJsonOptionsProvider)
{
    protected override RawItemTypesService? CreateServiceImpl(RawItemType[] data, CancellationToken cancellationToken) => new(data);
}
