using System.Text.Json;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawSuperAreasServiceFactory : ParsedDataServiceFactory<RawSuperAreasService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawSuperAreasServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.SuperAreas)
    {
    }

    protected override async Task<RawSuperAreasService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawSuperArea[]? data = await JsonSerializer.DeserializeAsync<RawSuperArea[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawSuperAreasService(data);
    }
}
