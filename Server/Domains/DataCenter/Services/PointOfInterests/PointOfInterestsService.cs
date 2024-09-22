using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class PointOfInterestsService(IReadOnlyCollection<RawPointOfInterest> pois)
{
    readonly Dictionary<int, RawPointOfInterest> _pois = pois.ToDictionary(poi => poi.PoiId, poi => poi);

    public RawPointOfInterest? GetPointOfInterest(int id) => _pois.GetValueOrDefault(id);
    public IEnumerable<RawPointOfInterest> GetPointOfInterests() => _pois.Values;
}
