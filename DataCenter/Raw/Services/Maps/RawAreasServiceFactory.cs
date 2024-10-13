using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Maps;

/// <summary>
/// </summary>
public class RawAreasServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawAreasService, RawArea[]>(rawDataRepository, RawDataType.Areas, rawDataJsonOptionsProvider)
{
    protected override RawAreasService? CreateServiceImpl(RawArea[] data, CancellationToken cancellationToken) => new(data);
}
