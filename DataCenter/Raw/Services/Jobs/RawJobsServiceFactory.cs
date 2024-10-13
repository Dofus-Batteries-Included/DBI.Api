using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.Jobs;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.Jobs;

/// <summary>
/// </summary>
public class RawJobsServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawJobsService, RawJob[]>(rawDataRepository, RawDataType.Jobs, rawDataJsonOptionsProvider)
{
    protected override RawJobsService? CreateServiceImpl(RawJob[] data, CancellationToken cancellationToken) => new(data);
}
