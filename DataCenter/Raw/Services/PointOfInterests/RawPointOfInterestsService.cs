using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw.Services.PointOfInterests;

/// <summary>
/// </summary>
public class RawPointOfInterestsService(IReadOnlyCollection<RawPointOfInterest> pois)
{
    readonly Dictionary<int, RawPointOfInterest> _pois = pois.ToDictionary(poi => poi.PoiId, poi => poi);

    public RawPointOfInterest? GetPointOfInterest(int id) => _pois.GetValueOrDefault(id);
    public IEnumerable<RawPointOfInterest> GetPointOfInterests() => _pois.Values;
}
