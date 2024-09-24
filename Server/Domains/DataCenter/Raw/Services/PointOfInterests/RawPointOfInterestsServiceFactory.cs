using System.Text.Json;
using Server.Domains.DataCenter.Raw.Models;
using Server.Domains.DataCenter.Raw.Services.Internal;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Raw.Services.PointOfInterests;

public class RawPointOfInterestsServiceFactory : ParsedDataServiceFactory<RawPointOfInterestsService>
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RawPointOfInterestsServiceFactory(IRawDataRepository rawDataRepository) : base(rawDataRepository, RawDataType.PointOfInterest)
    {
    }

    protected override async Task<RawPointOfInterestsService?> CreateServiceImpl(IRawDataFile file, CancellationToken cancellationToken)
    {
        await using Stream stream = file.OpenRead();
        RawPointOfInterest[]? data = await JsonSerializer.DeserializeAsync<RawPointOfInterest[]>(stream, _jsonSerializerOptions, cancellationToken);
        return data == null ? null : new RawPointOfInterestsService(data);
    }
}
