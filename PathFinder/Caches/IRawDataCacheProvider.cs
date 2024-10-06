namespace DBI.PathFinder.Caches;

public interface IRawDataCacheProvider
{
    public Task<IRawDataCache> GetCacheAsync(string release, CancellationToken cancellationToken = default);
}
