using System.Text.Json;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.Maps;

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
