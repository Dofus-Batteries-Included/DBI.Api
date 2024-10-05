using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class WorldDataBuilder
{
    BuildMode _mode;
    RawWorldGraphService? _rawWorldGraphService;
    MapsService? _mapsService;
    WorldDataFromDdcGithubRepositoryOptions? _options;
    ILogger? _logger;

    WorldDataBuilder()
    {
    }

    public WorldDataBuilder UseLogger(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public async Task<IWorldDataProvider> BuildAsync(CancellationToken cancellationToken = default)
    {
        IWorldDataProvider provider = _mode switch
        {
            BuildMode.FromRawServices => new WorldDataFromRawServices(
                _rawWorldGraphService ?? throw new NullReferenceException("Raw world graph service cannot be null."),
                _mapsService ?? throw new NullReferenceException("Maps service cannot be null.")
            ),
            BuildMode.FromDdcGithubRepository => await WorldDataFromDdcGithubRepositoryOptions.BuildProvider(
                _options ?? throw new NullReferenceException("Options cannot be null."),
                _logger ?? NullLogger.Instance,
                cancellationToken
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(_mode), _mode, null)
        };

        return provider;
    }

    public static WorldDataBuilder FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) =>
        new()
        {
            _mode = BuildMode.FromRawServices,
            _rawWorldGraphService = rawWorldGraphService,
            _mapsService = mapsService
        };

    public static WorldDataBuilder FromDdcGithubRepository(Action<WorldDataFromDdcGithubRepositoryOptions>? configure = null)
    {
        WorldDataFromDdcGithubRepositoryOptions options = new();
        configure?.Invoke(options);

        return new WorldDataBuilder
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
