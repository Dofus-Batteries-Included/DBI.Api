using System.Text.Json;
using Server.Domains.DataCenter.Models.Raw;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Services.Internal;

namespace Server.Domains.DataCenter.Services.Maps;

public class RawMapsServiceFactory : ParsedDataServiceFactory<RawMapsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawMapsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.Maps)
    {
    }

    protected override async Task<RawMapsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        Dictionary<long, RawMap>? data = await JsonSerializer.DeserializeAsync<Dictionary<long, RawMap>>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawMapsService(data);
    }
}
