using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Internal;

namespace DBI.DataCenter.Raw.Services.WorldGraphs;

/// <summary>
/// </summary>
public class RawWorldGraphServiceFactory(IRawDataRepository rawDataRepository, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    : ParsedDataServiceFactory<RawWorldGraphService, RawWorldGraph>(rawDataRepository, RawDataType.WorldGraph, rawDataJsonOptionsProvider)
{
    protected override RawWorldGraphService? CreateServiceImpl(RawWorldGraph data, CancellationToken cancellationToken) => new(data);
}
