using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawMapPositionsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawMapPositionsService, RawMapPosition[]>(rawDataRepository, RawDataType.MapPositions, rawDataJsonOptionsProvider)
{
    protected override RawMapPositionsService? CreateServiceImpl(RawMapPosition[] data, CancellationToken cancellationToken) => new(data);
}
