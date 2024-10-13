using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawSubAreasServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawSubAreasService, RawSubArea[]>(rawDataRepository, RawDataType.SubAreas, rawDataJsonOptionsProvider)
{
    protected override RawSubAreasService? CreateServiceImpl(RawSubArea[] data, CancellationToken cancellationToken) => new(data);
}
