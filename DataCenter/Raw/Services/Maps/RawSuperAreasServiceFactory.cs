using System.Text.Json;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;
using DBI.DataCenter.Repositories;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
