using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawMapsServiceFactory : ParsedDataServiceFactory<RawMapsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) }
    };

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
