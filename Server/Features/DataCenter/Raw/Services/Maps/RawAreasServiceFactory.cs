using System.Text.Json;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.Maps;

public class RawAreasServiceFactory : ParsedDataServiceFactory<RawAreasService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawAreasServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.Areas)
    {
    }

    protected override async Task<RawAreasService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawArea[]? data = await JsonSerializer.DeserializeAsync<RawArea[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawAreasService(data);
    }
}
