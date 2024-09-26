using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.WorldGraphs;

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
