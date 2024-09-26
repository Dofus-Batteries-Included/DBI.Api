using System.Text.Json;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawMapPositionsServiceFactory : ParsedDataServiceFactory<RawMapPositionsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawMapPositionsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.MapPositions)
    {
    }

    protected override async Task<RawMapPositionsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawMapPosition[]? data = await JsonSerializer.DeserializeAsync<RawMapPosition[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawMapPositionsService(data);
    }
}
