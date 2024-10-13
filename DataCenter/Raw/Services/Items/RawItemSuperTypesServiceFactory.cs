using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemSuperTypesServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawItemSuperTypesService, RawItemSuperType[]>(rawDataRepository, RawDataType.ItemSuperTypes, rawDataJsonOptionsProvider)
{
    protected override RawItemSuperTypesService? CreateServiceImpl(RawItemSuperType[] data, CancellationToken cancellationToken) => new(data);
}
