using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class PointOfInterestsService(IReadOnlyCollection<PointOfInterest> pois)
{
    readonly Dictionary<int, PointOfInterest> _pois = pois.ToDictionary(poi => poi.PoiId, poi => poi);

    public PointOfInterest? GetPointOfInterest(int id) => _pois.GetValueOrDefault(id);
    public IEnumerable<PointOfInterest> GetPointOfInterests() => _pois.Values;
}
