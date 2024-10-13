using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawMapsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawMapsService, Dictionary<long, RawMap>>(rawDataRepository, RawDataType.Maps, rawDataJsonOptionsProvider)
{
    protected override RawMapsService? CreateServiceImpl(Dictionary<long, RawMap> data, CancellationToken cancellationToken) => new(data);
}
