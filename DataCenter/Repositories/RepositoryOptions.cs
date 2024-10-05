namespace DBI.DataCenter.Repositories;

/// <summary>
///     Repository options.
/// </summary>
public class RepositoryOptions
{
    /// <summary>
    ///     The base path of the repository.
    /// </summary>
    public string BasePath { get; set; } = Path.GetFullPath("AppData");

    /// <summary>
    ///     The path used by the Data Center features.
    /// </summary>
    public string DataCenterPath => Path.Join(BasePath, "DataCenter");

    /// <summary>
    ///     The path used by the raw data of the Data Center features.
    /// </summary>
    public string DataCenterRawDataPath => Path.Join(DataCenterPath, "Raw");
}
