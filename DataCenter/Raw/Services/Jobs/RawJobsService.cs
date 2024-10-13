using DBI.DataCenter.Raw.Models.Jobs;

namespace DBI.DataCenter.Raw.Services.Jobs;

/// <summary>
/// </summary>
public class RawJobsService(IReadOnlyCollection<RawJob> jobs)
{
    readonly Dictionary<int, RawJob> _jobs = jobs.ToDictionary(job => job.Id, job => job);

    public RawJob? GetJob(int jobId) => _jobs.GetValueOrDefault(jobId);
    public IEnumerable<RawJob> GetJobs() => _jobs.Values;
}
