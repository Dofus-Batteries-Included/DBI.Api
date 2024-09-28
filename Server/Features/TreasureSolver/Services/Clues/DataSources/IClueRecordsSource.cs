using Server.Features.TreasureSolver.Models;

namespace Server.Features.TreasureSolver.Services.Clues.DataSources;

public interface IClueRecordsSource
{
    public Task<DateTime?> GetLastModificationDate();
    public Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId);
    Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData();
}
