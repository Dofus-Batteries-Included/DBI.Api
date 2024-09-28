using System.Text.Json;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawSubAreasServiceFactory : ParsedDataServiceFactory<RawSubAreasService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawSubAreasServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.SubAreas)
    {
    }

    protected override async Task<RawSubAreasService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawSubArea[]? data = await JsonSerializer.DeserializeAsync<RawSubArea[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawSubAreasService(data);
    }
}
