using System.Text.Json;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;
using DBI.DataCenter.Repositories;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
