using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DBI.PathFinder.Caches;

public class RawDataCacheProviderOnDisk : IRawDataCacheProvider
{
    readonly string _folder;
    readonly ILogger _logger;

    public RawDataCacheProviderOnDisk(string folder, ILogger? logger = null)
    {
        _folder = folder;
        _logger = logger ?? NullLogger.Instance;
    }

    public Task<IRawDataCache> GetCacheAsync(string release, CancellationToken cancellationToken = default)
    {
        string path = GetPath(release);
        return Task.FromResult<IRawDataCache>(new RawDataCacheOnDisk(path, _logger));
    }

    string GetPath(string release) => Path.Join(_folder, release);
}
