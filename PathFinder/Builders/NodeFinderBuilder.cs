using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.PathFinder.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class NodeFinderBuilder
{
    BuildMode _mode;
    RawWorldGraphService? _rawWorldGraphService;
    MapsService? _mapsService;
    WorldDataProviderFromDdcGithubRepositoryOptions? _options;
    ILogger? _logger;

    NodeFinderBuilder()
    {
    }

    public NodeFinderBuilder UseLogger(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public async Task<NodeFinder> BuildAsync(CancellationToken cancellationToken = default)
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

        return new NodeFinder(provider);
    }

    public static NodeFinderBuilder FromRawServices(RawWorldGraphService rawWorldGraphService, MapsService mapsService) =>
        new()
        {
            _mode = BuildMode.FromRawServices,
            _rawWorldGraphService = rawWorldGraphService,
            _mapsService = mapsService
        };

    public static NodeFinderBuilder FromDdcGithubRepository(Action<WorldDataProviderFromDdcGithubRepositoryOptions>? configure = null)
    {
        WorldDataProviderFromDdcGithubRepositoryOptions options = new();
        configure?.Invoke(options);

        return new NodeFinderBuilder
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
