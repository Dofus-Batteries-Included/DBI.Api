using Server.Features.TreasureSolver.Models;

namespace Server.Features.TreasureSolver.Services.Clues.DataSources;

class StaticClueRecordsSource : IClueRecordsSource
{
    public StaticClueRecordsSource(IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> clues, DateTime? lastModificationDate = null)
    {
        Clues = clues;
        LastModificationDate = lastModificationDate;
    }

    public DateTime? LastModificationDate { get; }
    public IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> Clues { get; }

    public Task<DateTime?> GetLastModificationDate() => Task.FromResult(LastModificationDate);
    public Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId) => Task.FromResult(Clues.GetValueOrDefault(mapId) ?? []);
    public Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData() => Task.FromResult(Clues);
}
