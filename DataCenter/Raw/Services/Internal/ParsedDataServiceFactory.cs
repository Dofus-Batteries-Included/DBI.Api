using System.Text.Json;
using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.Internal;

/// <summary>
///     Base class for all factories of services that expose raw data.
/// </summary>
public abstract class ParsedDataServiceFactory<TService, TData>
{
    readonly IRawDataRepository _rawDataRepository;
    readonly RawDataJsonOptionsProvider _rawDataJsonOptionsProvider;
    readonly Dictionary<string, TService> _cache = new();

    /// <summary>
    /// </summary>
    public ParsedDataServiceFactory(IRawDataRepository rawDataRepository, RawDataType dataType, RawDataJsonOptionsProvider rawDataJsonOptionsProvider)
    {
        _rawDataRepository = rawDataRepository;
        _rawDataJsonOptionsProvider = rawDataJsonOptionsProvider;
        DataType = dataType;
    }

    /// <summary>
    ///     The raw data exposed by the service.
    /// </summary>
    protected RawDataType DataType { get; }

    /// <summary>
    ///     Create the service for the given version now and cache it.
    /// </summary>
    public async Task PreloadAsync(string version, CancellationToken cancellationToken = default) => _ = await CreateServiceAsync(version, cancellationToken);

    /// <summary>
    ///     Create the service for the given version.
    /// </summary>
    public async Task<TService> CreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default)
    {
        (string? actualVersion, TService? service) = await TryCreateServiceImplAsync(version, cancellationToken);
        return service ?? throw new InvalidOperationException($"Could not create service of type {typeof(TService)} for version {version} (actual version: {actualVersion}).");
    }

    /// <inheritdoc cref="CreateServiceAsync" />
    public async Task<TService?> TryCreateServiceAsync(string version = "latest", CancellationToken cancellationToken = default) =>
        (await TryCreateServiceImplAsync(version, cancellationToken)).Service;

    async Task<(string? ActualVersion, TService? Service)> TryCreateServiceImplAsync(string version = "latest", CancellationToken cancellationToken = default)
    {
        string? actualVersion = version switch
        {
            "latest" => await _rawDataRepository.GetLatestVersionAsync(),
            _ => version
        };

        if (actualVersion == null)
        {
            return (null, default);
        }

        if (_cache.TryGetValue(actualVersion, out TService? service))
        {
            return (actualVersion, service);
        }

        IRawDataFile? file = await _rawDataRepository.TryGetRawDataFileAsync(actualVersion, DataType, cancellationToken);
        if (file == null)
        {
            return (actualVersion, default);
        }

        JsonSerializerOptions jsonSerializerOptions = file.DdcVersion == null
            ? _rawDataJsonOptionsProvider.GetJsonSerializerOptions(actualVersion, DataType)
            : _rawDataJsonOptionsProvider.GetJsonSerializerOptions(file.DdcVersion, actualVersion, DataType);

        await using Stream stream = file.OpenRead();
        TData? data = await JsonSerializer.DeserializeAsync<TData>(stream, jsonSerializerOptions, cancellationToken);
        if (data == null)
        {
            return (actualVersion, default);
        }

        TService? result = CreateServiceImpl(data, cancellationToken);
        if (result == null)
        {
            return (actualVersion, default);
        }

        _cache[actualVersion] = result;

        return (actualVersion, result);
    }

    /// <summary>
    ///     Actual implementation of the service creation.
    /// </summary>
    protected abstract TService? CreateServiceImpl(TData file, CancellationToken cancellationToken);
}
