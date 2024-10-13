using DBI.DataCenter.Raw.Models.Jobs;
using DBI.DataCenter.Raw.Services.I18N;
using DBI.DataCenter.Raw.Services.Jobs;
using DBI.DataCenter.Structured.Models.Jobs;

namespace DBI.DataCenter.Structured.Services.Jobs;

public class JobsService(RawJobsService? rawJobsService, LanguagesService? languagesService)
{
    public IEnumerable<Job>? GetJobs() => rawJobsService?.GetJobs().Select(Cook);

    public Job? GetJob(int jobId)
    {
        RawJob? rawJob = rawJobsService?.GetJob(jobId);
        return rawJob == null ? null : Cook(rawJob);
    }

    Job Cook(RawJob job) =>
        new()
        {
            Id = job.Id,
            Name = languagesService?.Get(job.NameId),
            HasLegendaryCraft = job.HasLegendaryCraft
        };
}
