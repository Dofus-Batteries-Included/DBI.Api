namespace DBI.PathFinder.Caches;

public class RawDataCacheProviderOnDisk : IRawDataCacheProvider
{
    readonly string _folder;

    public RawDataCacheProviderOnDisk(string folder)
    {
        _folder = folder;
    }

    public Task<IRawDataCache?> FindCacheAsync(string release, CancellationToken cancellationToken = default)
    {
        string path = GetPath(release);
        if (!Directory.Exists(path))
        {
            return Task.FromResult<IRawDataCache?>(null);
        }

        return Task.FromResult<IRawDataCache?>(new RawDataCacheOnDisk(path));
    }

    string GetPath(string release) => Path.Join(_folder, release);
}
