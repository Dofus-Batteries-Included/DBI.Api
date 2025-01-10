using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Features.TreasureSolver.Controllers.Requests;
using DBI.Server.Features.TreasureSolver.Models.Entities;
using DBI.Server.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace DBI.Server.Features.TreasureSolver.Services.Clues;

/// <summary>
///     Register clues.
/// </summary>
public class RegisterCluesService
{
    readonly ApplicationDbContext _context;

    /// <summary>
    /// </summary>
    public RegisterCluesService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Register that the <c>author</c> has found (or not found) the clues in <c>request</c>.
    /// </summary>
    public async Task RegisterCluesAsync(PrincipalEntity author, RegisterCluesRequest request, CancellationToken cancellationToken = default)
    {
        foreach (RegisterClueRequest clueRequest in request.Clues)
        {
            ClueAtMapStatus status = clueRequest.Found ? ClueAtMapStatus.Found : ClueAtMapStatus.NotFound;

            ClueRecordEntity? existingClue = await _context.ClueRecords.SingleOrDefaultAsync(
                c => c.MapId == clueRequest.MapId && c.ClueId == clueRequest.ClueId && c.Author == author,
                cancellationToken
            );
            if (existingClue != null)
            {
                existingClue.UpdateStatus(status);
                continue;
            }

            ClueRecordEntity newClue = new(clueRequest.ClueId, clueRequest.MapId, author, status);
            await _context.AddAsync(newClue, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
