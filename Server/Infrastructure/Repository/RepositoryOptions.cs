namespace Server.Infrastructure.Repository;

public class RepositoryOptions
{
    public string BasePath { get; set; } = Path.GetFullPath("AppData");
}
