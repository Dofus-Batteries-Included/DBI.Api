namespace DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;

/// <summary>
///     Manage known clues data sources.
/// </summary>
public class StaticCluesDataSourcesService
{
    readonly Dictionary<StaticCluesDataSourceName, IClueRecordsSource> _staticCluesDataSources = [];
    readonly object _lock = new();

    /// <summary>
    ///     Register a data source.
    /// </summary>
    public void RegisterDataSource(StaticCluesDataSourceName name, IClueRecordsSource clueRecordsSource)
    {
        lock (_lock)
        {
            _staticCluesDataSources[name] = clueRecordsSource;
        }
    }

    /// <summary>
    ///     Get all the data sources.
    /// </summary>
    /// <returns></returns>
    public IReadOnlyCollection<IClueRecordsSource> GetDataSources()
    {
        lock (_lock)
        {
            return _staticCluesDataSources.Values.ToArray();
        }
    }
}

/// <summary>
///     The known clues data sources
/// </summary>
public enum StaticCluesDataSourceName
{
    /// <summary>
    ///     Data from Dofus pour les noobs
    /// </summary>
    Dpln
}
