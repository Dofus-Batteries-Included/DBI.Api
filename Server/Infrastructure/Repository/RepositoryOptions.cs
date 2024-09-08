namespace Server.Infrastructure.Repository;

public class RepositoryOptions
{
    public string BasePath { get; set; } = Path.GetFullPath("AppData");

    public string DataCenterPath => Path.Join(BasePath, "DataCenter");
    public string DataCenterRawDataPath => Path.Join(DataCenterPath, "Raw");
}
