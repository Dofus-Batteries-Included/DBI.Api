using System.Text.Json;
using Server.Common.Exceptions;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class PointOfInterestsServiceFactory
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    readonly IRawDataRepository _rawDataRepository;

    public PointOfInterestsServiceFactory(IRawDataRepository rawDataRepository)
    {
        _rawDataRepository = rawDataRepository;
    }

    public async Task<PointOfInterestsService> CreatePointOfInterestsService(string version = "latest", CancellationToken cancellationToken = default)
    {
        IRawDataFile file = await _rawDataRepository.GetRawDataFileAsync(version, RawDataType.PointOfInterest, cancellationToken);
        await using Stream stream = file.OpenRead();
        Dictionary<int, PointOfInterest>? data = await JsonSerializer.DeserializeAsync<Dictionary<int, PointOfInterest>>(stream, _jsonSerializerOptions, cancellationToken);
        if (data == null)
        {
            throw new BadRequestException($"Could not load POIs for version {version}");
        }

        return new PointOfInterestsService(data);
    }
}
