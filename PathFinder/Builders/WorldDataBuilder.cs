using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;

namespace DBI.PathFinder.Builders;

public static class WorldDataBuilder
{
    public static WorldDataBuilderFromRawServices FromRawServices(
        RawWorldGraphService rawWorldGraphService,
        RawMapsService rawMapsService,
        RawMapPositionsService rawMapPositionsService
    ) =>
        new(rawWorldGraphService, rawMapsService, rawMapPositionsService);

    public static WorldDataBuilderFromRawData FromRawData(RawWorldGraph rawWorldGraph, Dictionary<long, RawMap> rawMaps, IReadOnlyCollection<RawMapPosition> rawMapPositions) =>
        new(rawWorldGraph, rawMaps, rawMapPositions);

    public static WorldDataBuilderFromDdcGithubRepository FromDdcGithubRepository() => new();
}
