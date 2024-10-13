using DBI.DataCenter.Structured.Models.Jobs;
using DBI.DataCenter.Structured.Services.Jobs;
using DBI.Server.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace DBI.Server.Features.DataCenter.Controllers;

/// <summary>
///     Jobs endpoints
/// </summary>
[Route("data-center/versions/{gameVersion}/jobs")]
[OpenApiTag("Jobs")]
[ApiController]
public class JobsController : ControllerBase
{
    readonly JobServicesFactory _jobServicesFactory;

    /// <summary>
    /// </summary>
    public JobsController(JobServicesFactory jobServicesFactory)
    {
        _jobServicesFactory = jobServicesFactory;
    }

    /// <summary>
    ///     Get jobs
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Job>> GetJobs(string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        JobsService jobsService = await _jobServicesFactory.CreateJobsServiceAsync(gameVersion, cancellationToken);
        return jobsService.GetJobs() ?? throw new NotFoundException($"Could not find jobs in version {gameVersion}.");
    }

    /// <summary>
    ///     Get job
    /// </summary>
    [HttpGet("{jobId:int}")]
    public async Task<Job> GetJobs(int jobId, string gameVersion = "latest", CancellationToken cancellationToken = default)
    {
        JobsService jobsService = await _jobServicesFactory.CreateJobsServiceAsync(gameVersion, cancellationToken);
        return jobsService.GetJob(jobId) ?? throw new NotFoundException($"Could not find job in version {gameVersion}.");
    }
}
