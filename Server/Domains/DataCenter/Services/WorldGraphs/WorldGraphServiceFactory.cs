using System.Text.Json;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Models.WorldGraphs;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Services.Internal;

namespace Server.Domains.DataCenter.Services.WorldGraphs;

public class WorldGraphServiceFactory : ParsedDataServiceFactory<WorldGraphService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public WorldGraphServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.PointOfInterest)
    {
    }

    protected override async Task<WorldGraphService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        WorldGraph? data = await JsonSerializer.DeserializeAsync<WorldGraph>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new WorldGraphService(data);
    }
}
