using Server.Domains.DataCenter.Models;

namespace Server.Domains.DataCenter.Services.PointOfInterests;

public class PointOfInterestsService(Dictionary<int, PointOfInterest> clues)
{
    public PointOfInterest? GetPointOfInterest(int id) => clues.GetValueOrDefault(id);
    public IEnumerable<PointOfInterest> GetPointOfInterests() => clues.Values;
}
