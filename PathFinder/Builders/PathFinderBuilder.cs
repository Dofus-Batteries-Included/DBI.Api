using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;
using DBI.PathFinder.Strategies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class PathFinderBuilder
{
    BuildMode _mode;
    RawWorldGraphService? _rawWorldGraphService;
    MapsService? _mapsService;
    WorldDataProviderFromDdcGithubRepositoryOptions? _options;
    ILogger? _logger;

    PathFinderBuilder()
    {
    }

    public PathFinderBuilder UseLogger(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public async Task<PathFinder> BuildAsync(CancellationToken cancellationToken = default)
    {
        IWorldDataProvider provider = _mode switch
        {
            BuildMode.FromRawServices => new WorldDataFromRawServices(
                _rawWorldGraphService ?? throw new NullReferenceException("Raw world graph service cannot be null."),
                _mapsService ?? throw new NullReferenceException("Maps service cannot be null.")
            ),
            BuildMode.FromDdcGithubRepository => await WorldDataProviderFromDdcGithubRepositoryOptions.BuildProvider(
                _options ?? throw new NullReferenceException("Options cannot be null."),
                _logger ?? NullLogger.Instance,
                cancellationToken
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(_mode), _mode, null)
        };

        return new PathFinder(new AStar(provider, _logger ?? NullLogger.Instance), provider);
    }

    public static PathFinderBuilder FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) =>
        new()
        {
            _mode = BuildMode.FromRawServices,
            _rawWorldGraphService = rawWorldGraphService,
            _mapsService = mapsService
        };

    public static PathFinderBuilder FromDdcGithubRepository(Action<WorldDataProviderFromDdcGithubRepositoryOptions>? configure = null)
    {
        WorldDataProviderFromDdcGithubRepositoryOptions options = new();
        configure?.Invoke(options);

        return new PathFinderBuilder
        {
            _mode = BuildMode.FromDdcGithubRepository,
            _options = options
        };
    }

    enum BuildMode
    {
        FromRawServices,
        FromDdcGithubRepository
    }
}
