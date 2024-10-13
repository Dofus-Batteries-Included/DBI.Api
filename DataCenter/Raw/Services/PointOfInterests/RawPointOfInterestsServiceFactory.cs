using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.PointOfInterests;

/// <summary>
/// </summary>
public class RawPointOfInterestsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawPointOfInterestsService, RawPointOfInterest[]>(rawDataRepository, RawDataType.PointOfInterest, rawDataJsonOptionsProvider)
{
    protected override RawPointOfInterestsService? CreateServiceImpl(RawPointOfInterest[] data, CancellationToken cancellationToken) => new(data);
}
