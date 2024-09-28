using Server.Features.DataCenter.Raw.Models;

namespace Server.Features.DataCenter.Repositories;

/// <summary>
///     Raw data files repository.
///     This abstraction allows to retrieve files from any kind of storage, e.g. the local file system, or a remote object storage.
/// </summary>
public interface IRawDataRepository
{
    /// <summary>
    ///     Invoked whenever the latest version has changed.
    /// </summary>
    event EventHandler<LatestVersionChangedEventArgs> LatestVersionChanged;

    /// <summary>
    ///     Get the latest available version in the repository.
    /// </summary>
    Task<string> GetLatestVersionAsync();

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

/// <summary>
///     Arguments of the <see cref="IRawDataRepository.LatestVersionChanged" /> event.
/// </summary>
public class LatestVersionChangedEventArgs
{
    /// <summary>
    ///     The new latest version after it has changed.
    /// </summary>
    public required string NewLatestVersion { get; init; }
}
