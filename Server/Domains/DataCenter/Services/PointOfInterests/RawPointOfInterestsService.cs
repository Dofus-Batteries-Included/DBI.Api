using Server.Domains.DataCenter.Models.Raw;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class RawPointOfInterestsService(IReadOnlyCollection<RawPointOfInterest> pois)
{
    readonly Dictionary<int, RawPointOfInterest> _pois = pois.ToDictionary(poi => poi.PoiId, poi => poi);

    public RawPointOfInterest? GetPointOfInterest(int id) => _pois.GetValueOrDefault(id);
    public IEnumerable<RawPointOfInterest> GetPointOfInterests() => _pois.Values;
}
