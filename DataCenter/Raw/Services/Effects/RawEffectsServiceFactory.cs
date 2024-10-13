using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Effects;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Effects;

/// <summary>
/// </summary>
public class RawEffectsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawEffectsService, RawEffect[]>(rawDataRepository, RawDataType.Effects, rawDataJsonOptionsProvider)
{
    protected override RawEffectsService? CreateServiceImpl(RawEffect[] data, CancellationToken cancellationToken) => new(data);
}
