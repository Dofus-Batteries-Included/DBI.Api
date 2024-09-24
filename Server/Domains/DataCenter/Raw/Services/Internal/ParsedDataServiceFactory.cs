using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.Internal;

public abstract class ParsedDataServiceFactory<TService>
{
    readonly IRawDataRepository _rawDataRepository;
    readonly Dictionary<string, TService> _cache = new();

    public ParsedDataServiceFactory(IRawDataRepository rawDataRepository, RawDataType dataType)
    {
        _rawDataRepository = rawDataRepository;
        DataType = dataType;
    }

    protected RawDataType DataType { get; }

    public async Task<TService> CreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default)
    {
        string actualVersion = version switch
        {
            "latest" => await _rawDataRepository.GetLatestVersionAsync(),
            _ => version
        };

        if (_cache.TryGetValue(actualVersion, out TService? service))
        {
            return service;
        }

        IRawDataFile file = await _rawDataRepository.GetRawDataFileAsync(actualVersion, DataType, cancellationToken);
        TService? result = await CreateServiceImpl(file, cancellationToken);
        if (result == null)
        {
            throw new InvalidOperationException($"Could not create service of type {typeof(TService)} for version {version} (actual version: {actualVersion}).");
        }

        _cache[actualVersion] = result;

        return result;
    }

    protected abstract Task<TService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken);
}
