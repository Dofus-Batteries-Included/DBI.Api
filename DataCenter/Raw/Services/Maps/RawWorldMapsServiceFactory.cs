using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawWorldMapsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawWorldMapsService, RawWorldMap[]>(rawDataRepository, RawDataType.WorldMaps, rawDataJsonOptionsProvider)
{
    protected override RawWorldMapsService? CreateServiceImpl(RawWorldMap[] data, CancellationToken cancellationToken) => new(data);
}
