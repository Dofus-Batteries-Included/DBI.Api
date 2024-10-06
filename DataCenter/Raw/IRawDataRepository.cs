using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw;

/// <summary>
///     Raw data files repository.
///     This abstraction allows to retrieve files from any kind of storage, e.g. the local file system, or a remote object storage.
/// </summary>
public interface IRawDataRepository
{
    /// <summary>
    ///     Get the latest available version in the repository.
    /// </summary>
    Task<string?> GetLatestVersionAsync();

    /// <summary>
    ///     Get the available versions in the repository.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<string>> GetAvailableVersionsAsync();

    /// <summary>
    ///     Try to get the file containing the requested data for the requested version of the game.
    /// </summary>
    Task<IRawDataFile?> TryGetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get the file containing the requested data for the requested version of the game.
    /// </summary>
    Task<IRawDataFile> GetRawDataFileAsync(string version, RawDataType type, CancellationToken cancellationToken = default);
}
