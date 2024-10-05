using System.Text.Json;
using DBI.Server.Features.DataCenter.Raw.Models;
using DBI.Server.Features.DataCenter.Raw.Services.Internal;
using DBI.Server.Features.DataCenter.Repositories;

namespace DBI.Server.Features.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldMapsServiceFactory : ParsedDataServiceFactory<RawWorldMapsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawWorldMapsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.WorldMaps)
    {
    }

    protected override async Task<RawWorldMapsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawWorldMap[]? data = await JsonSerializer.DeserializeAsync<RawWorldMap[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawWorldMapsService(data);
    }
}
