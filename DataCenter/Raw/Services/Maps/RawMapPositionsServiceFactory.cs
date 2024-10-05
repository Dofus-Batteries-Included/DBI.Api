using System.Text.Json;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;
using DBI.DataCenter.Repositories;

namespace DBI.DataCenter.Raw.Services.Maps;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
