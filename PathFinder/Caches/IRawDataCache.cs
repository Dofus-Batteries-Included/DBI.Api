using DBI.DataCenter.Raw.Models;

namespace DBI.PathFinder.Caches;

public interface IRawDataCache
{
    public Task<bool> ContainsDataAsync(RawDataType rawDataType, CancellationToken cancellationToken = default);
    public Task<Stream?> LoadDataAsync(RawDataType rawDataType, CancellationToken cancellationToken = default);
    public Task SaveDataAsync(RawDataType rawDataType, Stream stream, CancellationToken cancellationToken = default);
}
