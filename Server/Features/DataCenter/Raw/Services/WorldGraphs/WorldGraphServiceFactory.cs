using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class WorldGraphServiceFactory : ParsedDataServiceFactory<WorldGraphService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower, PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) }
    };

    public WorldGraphServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.WorldGraph)
    {
    }

    protected override async Task<WorldGraphService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        WorldGraph? data = await JsonSerializer.DeserializeAsync<WorldGraph>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new WorldGraphService(data);
    }
}
