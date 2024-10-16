﻿using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Serialization;
using DBI.Ddc;
using DBI.PathFinder.Caches;
using DBI.PathFinder.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Builders;

public class WorldDataBuilderFromDdcGithubRepository
{
    static readonly JsonSerializerOptions KebabCaseSerializerOptions = new()
    {
        PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(KebabCaseNamingPolicy.Instance) }
    };

    static readonly JsonSerializerOptions CamelCaseSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    string _githubRepository = "Dofus-Batteries-Included/DDC";
    IRawDataCacheProvider? _cacheProvider;
    ILogger? _logger;

    /// <summary>
    ///     Set the repository to use: either the original DDC repository or a fork.
    ///     Defaults to the original repository <c>Dofus-Batteries-Included/DDC</c>
    /// </summary>
    public WorldDataBuilderFromDdcGithubRepository UseFork(string repository)
    {
        _githubRepository = repository;
        return this;
    }

    /// <summary>
    ///     Set the cache provider to use to avoid downloading data from Github if possible.
    /// </summary>
    public WorldDataBuilderFromDdcGithubRepository UseCache(IRawDataCacheProvider cacheProvider)
    {
        _cacheProvider = cacheProvider;
        return this;
    }

    public WorldDataBuilderFromDdcGithubRepository UseLogger(ILogger logger)
    {
        _logger = logger;
        return this;
    }

    public async Task<IWorldDataProvider> BuildAsync(CancellationToken cancellationToken = default)
    {
        ILogger logger = _logger ?? NullLogger.Instance;
        DdcClient ddcClient = new(logger)
        {
            GithubRepository = _githubRepository
        };

        DdcRelease[] releases = await ddcClient.GetReleasesAsync(cancellationToken).ToArrayAsync(cancellationToken);
        if (releases.Length == 0)
        {
            throw new InvalidOperationException($"Could not find any release in repository {_githubRepository}.");
        }

        DdcRelease[] orderedReleases = releases.OrderByDescending(r => r.Name).ToArray();

        WorldData? worldData = null;
        foreach (DdcRelease release in orderedReleases)
        {
            if (_cacheProvider != null)
            {
                worldData = await TryGetWorldDataFromCacheProviderAsync(_cacheProvider, release, logger, cancellationToken);
                if (worldData != null)
                {
                    logger.LogInformation("Using cached files for release {Release}.", release.Name);
                    break;
                }
            }

            worldData = await TryGetWorldDataFromDdcGithubReleaseAsync(ddcClient, release, logger, cancellationToken);
            if (worldData != null)
            {
                logger.LogInformation("Using new data from release {Release}.", release.Name);

                if (_cacheProvider != null)
                {
                    await SaveWorldDataToCacheAsync(_cacheProvider, release, worldData, cancellationToken);
                }

                break;
            }
        }

        if (worldData == null)
        {
            throw new InvalidOperationException("Could not find world data in DDC Github repository.");
        }

        return WorldDataBuilder.FromRawData(worldData.WorldGraph, worldData.Maps, worldData.MapPositions).Build();
    }

    static async Task<WorldData?> TryGetWorldDataFromCacheProviderAsync(
        IRawDataCacheProvider cacheProvider,
        DdcRelease release,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        IRawDataCache cache = await cacheProvider.GetCacheAsync(release.Name, cancellationToken);

        if (!await cache.ContainsDataAsync(RawDataType.WorldGraph, cancellationToken)
            || !await cache.ContainsDataAsync(RawDataType.Maps, cancellationToken)
            || !await cache.ContainsDataAsync(RawDataType.MapPositions, cancellationToken))
        {
            logger.LogInformation("Could not find cached data for release {Release}.", release.Name);
            return null;
        }

        return await TryGetWorldDataFromCacheAsync(cache, release, logger, cancellationToken);
    }

    static async Task<WorldData?> TryGetWorldDataFromCacheAsync(IRawDataCache cache, DdcRelease release, ILogger logger, CancellationToken cancellationToken)
    {
        RawWorldGraph? worldGraph = await ExtractDataFromStreamAsync<RawWorldGraph>(cache, RawDataType.WorldGraph, CamelCaseSerializerOptions, cancellationToken);
        if (worldGraph == null)
        {
            logger.LogWarning("Could not find cached world graph for release {Release}.", release.Name);
            return null;
        }

        Dictionary<long, RawMap>? maps = await ExtractDataFromStreamAsync<Dictionary<long, RawMap>>(cache, RawDataType.Maps, CamelCaseSerializerOptions, cancellationToken);
        if (maps == null)
        {
            logger.LogWarning("Could not find cached maps for release {Release}.", release.Name);
            return null;
        }

        RawMapPosition[]? mapPositions = await ExtractDataFromStreamAsync<RawMapPosition[]>(cache, RawDataType.MapPositions, CamelCaseSerializerOptions, cancellationToken);
        if (mapPositions == null)
        {
            logger.LogWarning("Could not find cached map positions for release {Release}.", release.Name);
            return null;
        }

        return new WorldData
        {
            WorldGraph = worldGraph,
            Maps = maps,
            MapPositions = mapPositions
        };
    }

    static async Task<WorldData?> TryGetWorldDataFromDdcGithubReleaseAsync(DdcClient ddcClient, DdcRelease release, ILogger logger, CancellationToken cancellationToken)
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

        RawWorldGraph? worldGraph = await ExtractDataFromStreamAsync<RawWorldGraph>(content, RawDataType.WorldGraph, KebabCaseSerializerOptions, cancellationToken);
        if (worldGraph == null)
        {
            logger.LogWarning("Could not download world graph from release {Release}.", release.Name);
            return null;
        }

        Dictionary<long, RawMap>? maps = await ExtractDataFromStreamAsync<Dictionary<long, RawMap>>(content, RawDataType.Maps, CamelCaseSerializerOptions, cancellationToken);
        if (maps == null)
        {
            logger.LogWarning("Could not download maps from release {Release}.", release.Name);
            return null;
        }

        RawMapPosition[]? mapPositions = await ExtractDataFromStreamAsync<RawMapPosition[]>(content, RawDataType.MapPositions, CamelCaseSerializerOptions, cancellationToken);
        if (mapPositions == null)
        {
            logger.LogWarning("Could not download map positions from release {Release}.", release.Name);
            return null;
        }

        return new WorldData
        {
            WorldGraph = worldGraph,
            Maps = maps,
            MapPositions = mapPositions
        };
    }

    static async Task<TData?> ExtractDataFromStreamAsync<TData>(DdcReleaseContent content, RawDataType type, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        Stream? file = await content.GetFileContentAsync(GetFilename(type));
        if (file == null)
        {
            return default;
        }

        return await ExtractDataFromStreamAsync<TData>(file, options, cancellationToken);
    }

    static async Task<TData?> ExtractDataFromStreamAsync<TData>(IRawDataCache cache, RawDataType type, JsonSerializerOptions options, CancellationToken cancellationToken)
    {
        Stream? file = await cache.LoadDataAsync(type, cancellationToken);
        if (file == null)
        {
            return default;
        }

        return await ExtractDataFromStreamAsync<TData>(file, options, cancellationToken);
    }

    static async Task<TData?> ExtractDataFromStreamAsync<TData>(Stream stream, JsonSerializerOptions options, CancellationToken cancellationToken) =>
        await JsonSerializer.DeserializeAsync<TData>(stream, options, cancellationToken);

    static async Task SaveWorldDataToCacheAsync(IRawDataCacheProvider cacheProvider, DdcRelease release, WorldData worldData, CancellationToken cancellationToken)
    {
        IRawDataCache cache = await cacheProvider.GetCacheAsync(release.Name, cancellationToken);

        await using (Stream worldGraphStream = new MemoryStream())
        {
            await SaveDataToStreamAsync(worldGraphStream, worldData.WorldGraph, CamelCaseSerializerOptions, cancellationToken);
            worldGraphStream.Position = 0;
            await cache.SaveDataAsync(RawDataType.WorldGraph, worldGraphStream, cancellationToken);
        }

        await using (Stream mapsStream = new MemoryStream())
        {
            await SaveDataToStreamAsync(mapsStream, worldData.Maps, CamelCaseSerializerOptions, cancellationToken);
            mapsStream.Position = 0;
            await cache.SaveDataAsync(RawDataType.Maps, mapsStream, cancellationToken);
        }

        await using (Stream mapPositionsStream = new MemoryStream())
        {
            await SaveDataToStreamAsync(mapPositionsStream, worldData.MapPositions, CamelCaseSerializerOptions, cancellationToken);
            mapPositionsStream.Position = 0;
            await cache.SaveDataAsync(RawDataType.MapPositions, mapPositionsStream, cancellationToken);
        }
    }

    static async Task SaveDataToStreamAsync<TData>(Stream stream, TData value, JsonSerializerOptions options, CancellationToken cancellationToken) =>
        await JsonSerializer.SerializeAsync(stream, value, options, cancellationToken);

    static string GetFilename(RawDataType type) =>
        type switch
        {
            RawDataType.MapPositions => "map-positions.json",
            RawDataType.WorldGraph => "world-graph.json",
            RawDataType.Maps => "maps.json",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

    class WorldData
    {
        public required RawWorldGraph WorldGraph { get; init; }
        public required Dictionary<long, RawMap> Maps { get; init; }
        public required RawMapPosition[] MapPositions { get; init; }
    }
}
