using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Maps;
using DBI.DataCenter.Raw.Services.PointOfInterests;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;

namespace DBI.Server.Features.TreasureSolver.Services.Clues;

/// <summary>
///     Find clues.
/// </summary>
public class FindCluesService
{
    readonly IClueRecordsSource[] _sources;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly LanguagesServiceFactory _languagesServiceFactory;
    readonly RawPointOfInterestsServiceFactory _rawPointOfInterestsServiceFactory;
    readonly RawMapPositionsServiceFactory _rawMapPositionsServiceFactory;

    /// <summary>
    /// </summary>
    public FindCluesService(
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        LanguagesServiceFactory languagesServiceFactory,
        RawPointOfInterestsServiceFactory rawPointOfInterestsServiceFactory,
        RawMapPositionsServiceFactory rawMapPositionsServiceFactory
    )
    {
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _languagesServiceFactory = languagesServiceFactory;
        _rawPointOfInterestsServiceFactory = rawPointOfInterestsServiceFactory;
        _rawMapPositionsServiceFactory = rawMapPositionsServiceFactory;
        _sources = sources.ToArray();
    }

    /// <summary>
    ///     Get the last modification date of all the data used by the service.
    /// </summary>
    public async Task<DateTime?> GetLastModificationDateAsync(CancellationToken cancellationToken = default)
    {
        DateTime?[] dates = await Task.WhenAll(GetDataSources().Select(s => s.GetLastModificationDate(cancellationToken)));
        return dates.Max();
    }

    /// <summary>
    ///     Get all the clues in the given map from all the data sources.
    /// </summary>
    /// <remarks>
    ///     Different data sources might contradict, for example a principal might have registered a record saying that a given clue is not found in a given map.
    ///     For now, we resolve contradictions by taking the information that has the most recent modification date.
    /// </remarks>
    public async Task<IReadOnlyCollection<Clue>> FindCluesInMapAsync(long mapId, CancellationToken cancellationToken = default)
    {
        List<ClueRecord> results = [];
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyCollection<ClueRecord> cluesInMap = await source.GetCluesInMap(mapId, cancellationToken);
            results.AddRange(cluesInMap);
        }

        return await GetCluesFromRecordsAsync(results, cancellationToken);
    }

    /// <summary>
    ///     Get all the clues in maps at the given coordinates from all the data sources.
    /// </summary>
    /// <inheritdoc cref="FindCluesInMapAsync" />
    public async Task<IReadOnlyCollection<Clue>> FindCluesAtPositionAsync(int posX, int posY, string version = "latest", CancellationToken cancellationToken = default)
    {
        RawMapPositionsService rawMapPositionsService = await _rawMapPositionsServiceFactory.CreateServiceAsync(version, cancellationToken);
        long[] mapIds = rawMapPositionsService.GetMapPositions().Where(m => m.PosX == posX && m.PosY == posY).Select(m => m.MapId).ToArray();

        List<ClueRecord> results = [];
        foreach (long mapId in mapIds)
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyCollection<ClueRecord> cluesInMap = await source.GetCluesInMap(mapId, cancellationToken);
            results.AddRange(cluesInMap);
        }

        return await GetCluesFromRecordsAsync(results, cancellationToken);
    }

    async Task<IReadOnlyCollection<Clue>> GetCluesFromRecordsAsync(IEnumerable<ClueRecord> results, CancellationToken cancellationToken = default)
    {
        // TODO: maybe implement smarter conflict resolution, e.g. each data source can vote for their choice.

        LanguagesService languagesService = await _languagesServiceFactory.CreateLanguagesServiceAsync(cancellationToken: cancellationToken);
        RawPointOfInterestsService rawPointOfInterestsService = await _rawPointOfInterestsServiceFactory.CreateServiceAsync(cancellationToken: cancellationToken);
        ClueRecord[] records = results.GroupBy(r => r.ClueId).Select(g => g.OrderByDescending(r => r.RecordDate).First()).Where(r => r.Found).ToArray();
        return records.Select(
                r =>
                {
                    RawPointOfInterest? poi = rawPointOfInterestsService.GetPointOfInterest(r.ClueId);
                    return new Clue { ClueId = r.ClueId, Name = poi != null ? languagesService.Get(poi.NameId) : null };
                }
            )
            .ToArray();
    }

    IClueRecordsSource[] GetDataSources() => _sources.Concat(_staticCluesDataSourcesService.GetDataSources()).ToArray();
}
