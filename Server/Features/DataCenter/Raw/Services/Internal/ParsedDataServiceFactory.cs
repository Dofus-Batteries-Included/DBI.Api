using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Internal;

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

    public async Task PreloadAsync(string version, CancellationToken cancellationToken = default) => _ = await CreateServiceAsync(version, cancellationToken);

    public async Task<TService> CreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default)
    {
        (string? actualVersion, TService? service) = await TryCreateServiceImplAsync(version, cancellationToken);
        return service ?? throw new InvalidOperationException($"Could not create service of type {typeof(TService)} for version {version} (actual version: {actualVersion}).");
    }

    public async Task<TService?> TryCreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        (await TryCreateServiceImplAsync(version, cancellationToken)).Service;

    async Task<(string ActualVersion, TService? Service)> TryCreateServiceImplAsync(string version = "latest", CancellationToken cancellationToken = default)
    {
        string actualVersion = version switch
        {
            "latest" => await _rawDataRepository.GetLatestVersionAsync(),
            _ => version
        };

        if (_cache.TryGetValue(actualVersion, out TService? service))
        {
            return (actualVersion, service);
        }

        IRawDataFile? file = await _rawDataRepository.TryGetRawDataFileAsync(actualVersion, DataType, cancellationToken);
        if (file == null)
        {
            return (actualVersion, default);
        }

        TService? result = await CreateServiceImpl(file, cancellationToken);
        if (result == null)
        {
            return (actualVersion, default);
        }

        _cache[actualVersion] = result;

        return (actualVersion, result);
    }

    protected abstract Task<TService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken);
}
