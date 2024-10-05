using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Strategies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class PathFinderBuilder
{
    readonly IWorldDataProvider _worldDataProvider;
    ILogger? _logger;

    PathFinderBuilder(IWorldDataProvider worldDataProvider)
    {
        _worldDataProvider = worldDataProvider;
    }

    public PathFinderBuilder UseLogger(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public PathFinder Build() => new(new AStar(_worldDataProvider, _logger ?? NullLogger.Instance), _worldDataProvider);

    public static PathFinderBuilder FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) =>
        new(new WorldDataFromRawServices(rawWorldGraphService, mapsService));
}
