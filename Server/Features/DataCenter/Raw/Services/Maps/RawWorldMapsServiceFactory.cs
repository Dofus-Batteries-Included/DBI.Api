using System.Text.Json;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Maps;

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
