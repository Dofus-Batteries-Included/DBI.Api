namespace Server.Domains.TreasureSolver.Services.Clues.DataSources;

public class StaticCluesDataSourcesService
{
    readonly Dictionary<StaticCluesDataSourceName, IClueRecordsSource> _staticCluesDataSources = [];
    readonly object _lock = new();

    public void RegisterDataSource(StaticCluesDataSourceName name, IClueRecordsSource clueRecordsSource)
    {
        lock (_lock)
        {
            _staticCluesDataSources[name] = clueRecordsSource;
        }
    }

    public IReadOnlyCollection<IClueRecordsSource> GetDataSources()
    {
        lock (_lock)
        {
            return _staticCluesDataSources.Values.ToArray();
        }
    }
}

public enum StaticCluesDataSourceName
{
    Dpln
}
