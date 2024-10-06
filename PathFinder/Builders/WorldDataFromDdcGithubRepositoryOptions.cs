using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.Ddc;
using DBI.PathFinder.Caches;
using DBI.PathFinder.DataProviders;
using Microsoft.Extensions.Logging;

namespace DBI.PathFinder.Builders;

public class WorldDataFromDdcGithubRepositoryOptions
{
    static readonly JsonSerializerOptions KebabCaseSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) }
    };

    static readonly JsonSerializerOptions CamelCaseSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <summary>
    ///     The repository to use: either the original DDC repository or a fork.
    ///     Defaults to the original repository <c>Dofus-Batteries-Included/DDC</c>
    /// </summary>
    public string GithubRepository { get; set; } = "Dofus-Batteries-Included/DDC";

    public IRawDataCacheProvider? CacheProvider { get; set; } = null;

    internal static async Task<IWorldDataProvider> BuildProviderAsync(
        WorldDataFromDdcGithubRepositoryOptions options,
        ILogger logger,
        CancellationToken cancellationToken = default
    )
    {
        DdcClient ddcClient = new(logger)
        {
            GithubRepository = options.GithubRepository
        };

        DdcRelease[] releases = await ddcClient.GetReleasesAsync(cancellationToken).ToArrayAsync(cancellationToken);
        if (releases.Length == 0)
        {
            throw new InvalidOperationException($"Could not find any release in repository {options.GithubRepository}.");
        }

        DdcRelease[] orderedReleases = releases.OrderByDescending(r => r.Name).ToArray();

        foreach (DdcRelease release in orderedReleases)
        {
            if (options.CacheProvider != null)
            {
                IWorldDataProvider? providerFromCachedData = await BuildProviderFromCacheProviderAsync(options.CacheProvider, logger, release, cancellationToken);
                if (providerFromCachedData != null)
                {
                    logger.LogInformation("Using cached files for release {Release}.", release.Name);
                    return providerFromCachedData;
                }
            }

            IWorldDataProvider? providerFromRemoteData = await BuildProviderFromDdcGithubReleaseAsync(ddcClient, release, logger, cancellationToken);
            if (providerFromRemoteData != null)
            {
                logger.LogInformation("Using new data from release {Release}.", release.Name);
                return providerFromRemoteData;
            }
        }

        throw new InvalidOperationException("Could not find world data in DDC Github repository.");
    }

    static async Task<IWorldDataProvider?> BuildProviderFromCacheProviderAsync(
        IRawDataCacheProvider cacheProvider,
        ILogger logger,
        DdcRelease release,
        CancellationToken cancellationToken
    )
    {
        IRawDataCache? cache = await cacheProvider.FindCacheAsync(release.Name, cancellationToken);
        if (cache == null)
        {
            logger.LogInformation("Could not find cached data for release {Release}.", release.Name);
            return null;
        }

        if (!await cache.ContainsDataAsync(RawDataType.WorldGraph, cancellationToken)
            || !await cache.ContainsDataAsync(RawDataType.Maps, cancellationToken)
            || !await cache.ContainsDataAsync(RawDataType.MapPositions, cancellationToken))
        {
            logger.LogWarning("Found incomplete cache data for release {Release}.", release.Name);
            return null;
        }

        return await BuildProviderFromCacheAsync(cache, release, logger, cancellationToken);
    }

    static async Task<IWorldDataProvider?> BuildProviderFromCacheAsync(IRawDataCache cache, DdcRelease release, ILogger logger, CancellationToken cancellationToken)
    {
        RawWorldGraph? worldGraph = await ExtractDataFromFileAsync<RawWorldGraph>(cache, RawDataType.WorldGraph, KebabCaseSerializerOptions, cancellationToken);
        if (worldGraph == null)
        {
            logger.LogWarning("Could not find cached world graph for release {Release}.", release.Name);
            return null;
        }

        Dictionary<long, RawMap>? maps = await ExtractDataFromFileAsync<Dictionary<long, RawMap>>(cache, RawDataType.Maps, CamelCaseSerializerOptions, cancellationToken);
        if (maps == null)
        {
            logger.LogWarning("Could not find cached maps for release {Release}.", release.Name);
            return null;
        }

        RawMapPosition[]? mapPositions = await ExtractDataFromFileAsync<RawMapPosition[]>(cache, RawDataType.MapPositions, CamelCaseSerializerOptions, cancellationToken);
        if (mapPositions == null)
        {
            logger.LogWarning("Could not find cached map positions for release {Release}.", release.Name);
            return null;
        }

        RawWorldGraphService rawWorldGraphService = new(worldGraph);
        MapsService mapsService = new(null, null, null, null, new RawMapsService(maps), new RawMapPositionsService(mapPositions), null);

        return new WorldDataFromRawServices(rawWorldGraphService, mapsService);
    }

    static async Task<IWorldDataProvider?> BuildProviderFromDdcGithubReleaseAsync(DdcClient ddcClient, DdcRelease release, ILogger logger, CancellationToken cancellationToken)
    {
        if (release.Content == null)
        {
            logger.LogWarning("Could not find content asset in release {Release}.", release.Name);
            return null;
        }

        DdcReleaseContent? content = await ddcClient.DownloadReleaseContentAsync(release, cancellationToken);
        if (content == null)
        {
            logger.LogWarning("Could not download content asset from release {Release}.", release.Name);
            return null;
        }

        logger.LogInformation("Selected latest release with content: {Release}.", release.Name);

        RawWorldGraph? worldGraph = await ExtractDataFromFileAsync<RawWorldGraph>(content, RawDataType.WorldGraph, KebabCaseSerializerOptions, cancellationToken);
        if (worldGraph == null)
        {
            logger.LogWarning("Could not download world graph from release {Release}.", release.Name);
            return null;
        }

        Dictionary<long, RawMap>? maps = await ExtractDataFromFileAsync<Dictionary<long, RawMap>>(content, RawDataType.Maps, CamelCaseSerializerOptions, cancellationToken);
        if (maps == null)
        {
            logger.LogWarning("Could not download maps from release {Release}.", release.Name);
            return null;
        }

        RawMapPosition[]? mapPositions = await ExtractDataFromFileAsync<RawMapPosition[]>(content, RawDataType.MapPositions, CamelCaseSerializerOptions, cancellationToken);
        if (mapPositions == null)
        {
            logger.LogWarning("Could not download map positions from release {Release}.", release.Name);
            return null;
        }

        RawWorldGraphService rawWorldGraphService = new(worldGraph);
        MapsService mapsService = new(null, null, null, null, new RawMapsService(maps), new RawMapPositionsService(mapPositions), null);

        return new WorldDataFromRawServices(rawWorldGraphService, mapsService);
    }

    static async Task<TData?> ExtractDataFromFileAsync<TData>(DdcReleaseContent content, RawDataType type, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        Stream? file = await content.GetFileContentAsync(GetFilename(type));
        if (file == null)
        {
            return default;
        }

        return await ExtractDataFromFileAsync<TData>(file, options, cancellationToken);
    }

    static async Task<TData?> ExtractDataFromFileAsync<TData>(IRawDataCache cache, RawDataType type, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        Stream? file = await cache.LoadDataAsync(type, cancellationToken);
        if (file == null)
        {
            return default;
        }

        return await ExtractDataFromFileAsync<TData>(file, options, cancellationToken);
    }

    static async Task<TData?> ExtractDataFromFileAsync<TData>(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken) =>
        await JsonSerializer.DeserializeAsync<TData>(stream, options, cancellationToken);

    static string GetFilename(RawDataType type) =>
        type switch
        {
            RawDataType.MapPositions => "map-positions.json",
            RawDataType.WorldGraph => "world-graph.json",
            RawDataType.Maps => "maps.json",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
}
