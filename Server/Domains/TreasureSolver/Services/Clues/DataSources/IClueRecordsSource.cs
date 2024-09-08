using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.Clues.DataSources;

public interface IClueRecordsSource
{
    public Task<DateTime?> GetLastModificationDate();
    public Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId);
    Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData();
}
