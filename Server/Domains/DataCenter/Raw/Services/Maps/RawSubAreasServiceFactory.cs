using System.Text.Json;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

public class RawSubAreasServiceFactory : ParsedDataServiceFactory<RawSubAreasService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawSubAreasServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.SubAreas)
    {
    }

    protected override async Task<RawSubAreasService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        Dictionary<long, RawSubArea>? data = await JsonSerializer.DeserializeAsync<Dictionary<long, RawSubArea>>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawSubAreasService(data);
    }
}
