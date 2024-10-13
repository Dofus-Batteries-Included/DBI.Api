using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Items;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Items;

/// <summary>
/// </summary>
public class RawItemSetsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawItemSetsService, RawItemSet[]>(rawDataRepository, RawDataType.ItemSets, rawDataJsonOptionsProvider)
{
    protected override RawItemSetsService? CreateServiceImpl(RawItemSet[] data, CancellationToken cancellationToken) => new(data);
}
