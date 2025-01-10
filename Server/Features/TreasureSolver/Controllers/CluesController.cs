using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Features.TreasureSolver.Controllers.Requests;
using DBI.Server.Features.TreasureSolver.Models;
using DBI.Server.Features.TreasureSolver.Services.Clues;
using DBI.Server.Infrastructure.Authentication;
using DBI.Server.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DBI.Server.Features.TreasureSolver.Controllers;

/// <summary>
///     Clues endpoints
/// </summary>
[Route("treasure-solver/clues")]
[ApiController]
public class CluesController : ControllerBase
{
    /// <summary>
    ///     Find clues in map
    /// </summary>
    [HttpGet("at-map/{mapId:long}")]
    public async Task<IReadOnlyCollection<Clue>> FindCluesInMap(
        [FromServices] FindCluesService findCluesService,
        long mapId,
        string version = "latest",
        CancellationToken cancellationToken = default
    ) =>
        // FIXME: clues are not versioned right now so the version parameter is always ignored by this endpoint
        await findCluesService.FindCluesInMapAsync(mapId, cancellationToken);

    /// <summary>
    ///     Find clues at position
    /// </summary>
    [HttpGet("at-position/{posX:int}/{posY:int}")]
    public async Task<IReadOnlyCollection<Clue>> FindCluesAtPosition(
        [FromServices] FindCluesService findCluesService,
        int posX,
        int posY,
        string version = "latest",
        CancellationToken cancellationToken = default
    ) =>
        await findCluesService.FindCluesAtPositionAsync(posX, posY, version, cancellationToken);

    /// <summary>
    ///     Export clues
    /// </summary>
    [HttpGet("export")]
    public async Task<FileStreamResult> ExportClues([FromServices] ExportCluesService exportCluesService, string version = "latest", CancellationToken cancellationToken = default)
    {
        // FIXME: clues are not versioned right now so the version parameter is always ignored by this endpoint
        ExportCluesService.File file = await exportCluesService.ExportCluesAsync(cancellationToken);
        return new FileStreamResult(file.Content, file.Type) { FileDownloadName = file.Name };
    }

    /// <summary>
    ///     Register clues
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task RegisterClues(
        [FromServices] RegisterCluesService registerCluesService,
        [FromServices] ApplicationDbContext dbContext,
        RegisterCluesRequest request,
        string version = "latest",
        CancellationToken cancellationToken = default
    )
    {
        // FIXME: clues are not versioned right now so the version parameter is always ignored by this endpoint
        PrincipalEntity principal = await ControllerContext.RequirePrincipal(dbContext);
        await registerCluesService.RegisterCluesAsync(principal, request, cancellationToken);
    }
}
