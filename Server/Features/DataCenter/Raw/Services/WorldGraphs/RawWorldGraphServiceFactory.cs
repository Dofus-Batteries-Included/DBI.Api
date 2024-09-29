using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Features.DataCenter.Raw.Models;
using Server.Features.DataCenter.Raw.Models.WorldGraphs;
using Server.Features.DataCenter.Raw.Services.Internal;
using Server.Features.DataCenter.Repositories;

namespace Server.Features.DataCenter.Raw.Services.WorldGraphs;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawWorldGraphServiceFactory : ParsedDataServiceFactory<RawWorldGraphService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower, PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) }
    };

    public RawWorldGraphServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.WorldGraph)
    {
    }

    protected override async Task<RawWorldGraphService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawWorldGraph? data = await JsonSerializer.DeserializeAsync<RawWorldGraph>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawWorldGraphService(data);
    }
}
