using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;
using Server.Domains.TreasureSolver.Services.Maps;

namespace Server.Domains.TreasureSolver.Services.Clues;

public class FindCluesService
{
    readonly IClueRecordsSource[] _sources;
    readonly ICluesService _cluesService;
    readonly IMapsService _mapsService;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;

    public FindCluesService(
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        ICluesService cluesService,
        IMapsService mapsService
    )
    {
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _cluesService = cluesService;
        _mapsService = mapsService;
        _sources = sources.ToArray();
    }

    public async Task<DateTime?> GetLastModificationDate()
    {
        DateTime?[] dates = await Task.WhenAll(GetDataSources().Select(s => s.GetLastModificationDate()));
        return dates.Max();
    }

    public async Task<IReadOnlyCollection<Clue>> FindCluesInMapAsync(long mapId)
    {
        List<ClueRecord> results = [];
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyCollection<ClueRecord> cluesInMap = await source.GetCluesInMap(mapId);
            results.AddRange(cluesInMap);
        }

        return GetCluesFromRecords(results);
    }

    public async Task<IReadOnlyCollection<Clue>> FindCluesAtPositionAsync(int posX, int posY)
    {
        long[] mapIds = _mapsService.GetMaps().Where(m => m.PosX == posX && m.PosY == posY).Select(m => m.MapId).ToArray();

        List<ClueRecord> results = [];
        foreach (long mapId in mapIds)
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyCollection<ClueRecord> cluesInMap = await source.GetCluesInMap(mapId);
            results.AddRange(cluesInMap);
        }

        return GetCluesFromRecords(results);
    }

    IReadOnlyCollection<Clue> GetCluesFromRecords(IEnumerable<ClueRecord> results)
    {
        ClueRecord[] records = results.GroupBy(r => r.ClueId).Select(g => g.OrderByDescending(r => r.RecordDate).First()).Where(r => r.Found).ToArray();
        return records.Select(
                r =>
                {
                    Clue? clue = _cluesService.GetClue(r.ClueId);
                    return new Clue { ClueId = r.ClueId, Name = clue?.Name ?? new LocalizedText() };
                }
            )
            .ToArray();
    }

    IClueRecordsSource[] GetDataSources() => _sources.Concat(_staticCluesDataSourcesService.GetDataSources()).ToArray();
}
