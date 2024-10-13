using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawSuperAreasServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawSuperAreasService, RawSuperArea[]>(rawDataRepository, RawDataType.SuperAreas, rawDataJsonOptionsProvider)
{
    protected override RawSuperAreasService? CreateServiceImpl(RawSuperArea[] data, CancellationToken cancellationToken) => new(data);
}
