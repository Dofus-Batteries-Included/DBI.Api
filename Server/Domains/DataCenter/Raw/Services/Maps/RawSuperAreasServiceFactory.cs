using System.Text.Json;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawSuperAreasServiceFactory : ParsedDataServiceFactory<RawSuperAreasService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawSuperAreasServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.SuperAreas)
    {
    }

    protected override async Task<RawSuperAreasService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        Dictionary<long, RawSuperArea>? data = await JsonSerializer.DeserializeAsync<Dictionary<long, RawSuperArea>>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawSuperAreasService(data);
    }
}
