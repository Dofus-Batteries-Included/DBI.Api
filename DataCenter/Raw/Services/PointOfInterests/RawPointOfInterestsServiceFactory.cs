using System.Text.Json;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Raw.Services.Internal;
using DBI.DataCenter.Repositories;

namespace DBI.DataCenter.Raw.Services.PointOfInterests;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

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
