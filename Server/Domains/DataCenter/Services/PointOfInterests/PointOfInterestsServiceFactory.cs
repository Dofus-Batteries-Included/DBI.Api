using System.Text.Json;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Repositories;
using Server.Domains.DataCenter.Services.Internal;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class PointOfInterestsServiceFactory : ParsedDataServiceFactory<PointOfInterestsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public PointOfInterestsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.PointOfInterest)
    {
    }

    protected override async Task<PointOfInterestsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawPointOfInterest[]? data = await JsonSerializer.DeserializeAsync<RawPointOfInterest[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new PointOfInterestsService(data);
    }
}
