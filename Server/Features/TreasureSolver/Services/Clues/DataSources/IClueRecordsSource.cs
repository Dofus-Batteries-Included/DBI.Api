using DBI.Server.Features.TreasureSolver.Models;

namespace DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;

/// <summary>
///     A source of clue records.
/// </summary>
public interface IClueRecordsSource
{
    /// <summary>
    ///     Get the last modification date of the data in the source.
    /// </summary>
    public Task<DateTime?> GetLastModificationDate(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get the clues in the given map.
    /// </summary>
    public Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Build a dictionary containing all the clue records in the source.
    /// </summary>
    Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData(CancellationToken cancellationToken = default);
}
