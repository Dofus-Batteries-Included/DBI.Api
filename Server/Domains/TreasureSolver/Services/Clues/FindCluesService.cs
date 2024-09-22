using Server.Common.Models;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Services.I18N;
using Server.Domains.DataCenter.Services.Maps;
using Server.Domains.DataCenter.Services.PointOfInterests;
using Server.Domains.TreasureSolver.Models;
using Server.Domains.TreasureSolver.Services.Clues.DataSources;

namespace Server.Domains.TreasureSolver.Services.Clues;

public class FindCluesService
{
    readonly IClueRecordsSource[] _sources;
    readonly StaticCluesDataSourcesService _staticCluesDataSourcesService;
    readonly LanguagesServiceFactory _languagesServiceFactory;
    readonly PointOfInterestsServiceFactory _pointOfInterestsServiceFactory;
    readonly MapsServiceFactory _mapsServiceFactory;

    public FindCluesService(
        IEnumerable<IClueRecordsSource> sources,
        StaticCluesDataSourcesService staticCluesDataSourcesService,
        LanguagesServiceFactory languagesServiceFactory,
        PointOfInterestsServiceFactory pointOfInterestsServiceFactory,
        MapsServiceFactory mapsServiceFactory
    )
    {
        _staticCluesDataSourcesService = staticCluesDataSourcesService;
        _languagesServiceFactory = languagesServiceFactory;
        _pointOfInterestsServiceFactory = pointOfInterestsServiceFactory;
        _mapsServiceFactory = mapsServiceFactory;
        _sources = sources.ToArray();
    }

    public async Task<DateTime?> GetLastModificationDateAsync()
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

        return await GetCluesFromRecordsAsync(results);
    }

    public async Task<IReadOnlyCollection<Clue>> FindCluesAtPositionAsync(int posX, int posY)
    {
        MapsService mapsService = await _mapsServiceFactory.CreateService();
        long[] mapIds = mapsService.GetMaps().Where(m => m.PosX == posX && m.PosY == posY).Select(m => m.MapId).ToArray();

        List<ClueRecord> results = [];
        foreach (long mapId in mapIds)
        foreach (IClueRecordsSource source in GetDataSources())
        {
            IReadOnlyCollection<ClueRecord> cluesInMap = await source.GetCluesInMap(mapId);
            results.AddRange(cluesInMap);
        }

        return await GetCluesFromRecordsAsync(results);
    }

    async Task<IReadOnlyCollection<Clue>> GetCluesFromRecordsAsync(IEnumerable<ClueRecord> results)
    {
        LanguagesService languagesService = await _languagesServiceFactory.CreateLanguagesService();
        PointOfInterestsService pointOfInterestsService = await _pointOfInterestsServiceFactory.CreateService();
        ClueRecord[] records = results.GroupBy(r => r.ClueId).Select(g => g.OrderByDescending(r => r.RecordDate).First()).Where(r => r.Found).ToArray();
        return records.Select(
                r =>
                {
                    RawPointOfInterest? poi = pointOfInterestsService.GetPointOfInterest(r.ClueId);
                    return new Clue { ClueId = r.ClueId, Name = poi != null ? languagesService.Get(poi.NameId) : new LocalizedText() };
                }
            )
            .ToArray();
    }

    IClueRecordsSource[] GetDataSources() => _sources.Concat(_staticCluesDataSourcesService.GetDataSources()).ToArray();
}
