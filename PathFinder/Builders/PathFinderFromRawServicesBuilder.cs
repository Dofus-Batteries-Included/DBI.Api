using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Strategies;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class PathFinderFromRawServicesBuilder
{
    readonly IWorldDataProvider _worldDataProvider;

    internal PathFinderFromRawServicesBuilder(RawWorldGraphService rawWorldGraphService, MapsService mapsService)
    {
        _worldDataProvider = new WorldDataFromRawServices(rawWorldGraphService, mapsService);
    }

    public PathFinder Build() => new(new AStar(_worldDataProvider, NullLogger<AStar>.Instance), _worldDataProvider);
}
