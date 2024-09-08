using Server.Domains.TreasureSolver.Models;

namespace Server.Domains.TreasureSolver.Services.Maps;

public class MapsService : IMapsService
{
    IReadOnlyDictionary<long, Map> _maps = new Dictionary<long, Map>();
    public event EventHandler? DataRefreshed;

    public Map? GetMap(long id) => _maps.GetValueOrDefault(id);
    public IEnumerable<Map> GetMaps() => _maps.Values;

    public void SaveMaps(IReadOnlyDictionary<long, Map> maps)
    {
        _maps = maps;
        DataRefreshed?.Invoke(this, EventArgs.Empty);
    }
}
