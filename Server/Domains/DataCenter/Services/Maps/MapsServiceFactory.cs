using System.Text.Json;
using Server.Common.Exceptions;
using Server.Domains.DataCenter.Models;
using Server.Domains.DataCenter.Repositories;

namespace Server.Domains.DataCenter.Services.Maps;

public class MapsServiceFactory
{
    readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    readonly IRawDataRepository _rawDataRepository;

    public MapsServiceFactory(IRawDataRepository rawDataRepository)
    {
        _rawDataRepository = rawDataRepository;
    }

    public async Task<MapsService> CreateMapsService(string version = "latest", CancellationToken cancellationToken = default)
    {
        IRawDataFile file = await _rawDataRepository.GetRawDataFileAsync(version, RawDataType.MapPositions, cancellationToken);
        await using Stream stream = file.OpenRead();
        MapPositions[]? data = await JsonSerializer.DeserializeAsync<MapPositions[]>(stream, _jsonSerializerOptions, cancellationToken);
        if (data == null)
        {
            throw new BadRequestException($"Could not load maps for version {version}");
        }

        return new MapsService(data);
    }
}
