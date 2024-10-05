using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Models.WorldGraphs;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.WorldGraphs;
using DBI.DataCenter.Structured.Services;
using DBI.Ddc;
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

    internal static async Task<IWorldDataProvider> BuildProvider(WorldDataFromDdcGithubRepositoryOptions options, ILogger logger, CancellationToken cancellationToken = default)
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

        DdcRelease? selectedRelease = null;
        DdcReleaseContent? content = null;
        foreach (DdcRelease release in releases.OrderByDescending(r => r.Name))
        {
            selectedRelease = release;
            content = await ddcClient.DownloadReleaseContentAsync(release, cancellationToken);
            if (content != null)
            {
                break;
            }
        }

        if (selectedRelease == null || content == null)
        {
            throw new InvalidOperationException($"Could not get any content in the following releases: {string.Join(", ", releases.Select(r => r.Name))}.");
        }

        logger.LogInformation("Selected latest release with content: {Release}.", selectedRelease.Name);

        RawWorldGraph worldGraph = await ExtractDataFromFile<RawWorldGraph>(selectedRelease, content, "world-graph.json", KebabCaseSerializerOptions, cancellationToken);
        Dictionary<long, RawMap> maps = await ExtractDataFromFile<Dictionary<long, RawMap>>(selectedRelease, content, "maps.json", CamelCaseSerializerOptions, cancellationToken);
        RawMapPosition[] mapPositions = await ExtractDataFromFile<RawMapPosition[]>(selectedRelease, content, "map-positions.json", CamelCaseSerializerOptions, cancellationToken);

        RawWorldGraphService rawWorldGraphService = new(worldGraph);
        MapsService mapsService = new(null, null, null, null, new RawMapsService(maps), new RawMapPositionsService(mapPositions), null);

        return new WorldDataFromRawServices(rawWorldGraphService, mapsService);
    }

    static async Task<TData> ExtractDataFromFile<TData>(
        DdcRelease release,
        DdcReleaseContent content,
        string filename,
        JsonSerializerOptions options,
        CancellationToken cancellationToken
    )
    {
        Stream? file = await content.GetFileContentAsync(filename);
        if (file == null)
        {
            throw new InvalidOperationException($"Could not get content of file {filename} in release {release.Name}.");
        }

        TData? worldGraph = await JsonSerializer.DeserializeAsync<TData>(file, options, cancellationToken);
        if (worldGraph == null)
        {
            throw new InvalidOperationException($"Could not read content of file {filename} in release {release.Name}.");
        }

        return worldGraph;
    }
}
