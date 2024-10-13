using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawItemsService, RawItem[]>(rawDataRepository, RawDataType.Items, rawDataJsonOptionsProvider)
{
    protected override RawItemsService? CreateServiceImpl(RawItem[] data, CancellationToken cancellationToken) => new(data);
}
