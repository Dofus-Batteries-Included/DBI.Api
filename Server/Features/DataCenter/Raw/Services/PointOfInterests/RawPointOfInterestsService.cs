using DBI.Server.Features.DataCenter.Raw.Models;

namespace DBI.Server.Features.DataCenter.Raw.Services.PointOfInterests;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class RawPointOfInterestsService(IReadOnlyCollection<RawPointOfInterest> pois)
{
    readonly Dictionary<int, RawPointOfInterest> _pois = pois.ToDictionary(poi => poi.PoiId, poi => poi);

    public RawPointOfInterest? GetPointOfInterest(int id) => _pois.GetValueOrDefault(id);
    public IEnumerable<RawPointOfInterest> GetPointOfInterests() => _pois.Values;
}
