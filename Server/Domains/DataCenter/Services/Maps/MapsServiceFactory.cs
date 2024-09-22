using System.Text.Json;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Services.Internal;

namespace Server.Domains.DataCenter.Services.Maps;

public class MapsServiceFactory : ParsedDataServiceFactory<MapsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public MapsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.MapPositions)
    {
    }

    protected override async Task<MapsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawMapPosition[]? data = await JsonSerializer.DeserializeAsync<RawMapPosition[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new MapsService(data);
    }
}
