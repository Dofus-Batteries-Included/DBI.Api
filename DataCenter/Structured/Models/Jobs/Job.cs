using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Structured.Models.Jobs;

public class Job
{
    public int Id { get; set; }
    public LocalizedText? Name { get; set; }
    public bool HasLegendaryCraft { get; set; }
}
