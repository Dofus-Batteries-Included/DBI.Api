using Microsoft.EntityFrameworkCore;
using Server.Features.Identity.Models.Entities;
using Server.Features.TreasureSolver.Controllers.Requests;
using Server.Features.TreasureSolver.Models.Entities;
using Server.Infrastructure.Database;

namespace Server.Features.TreasureSolver.Services.Clues;

public class RegisterCluesService
{
    readonly ApplicationDbContext _context;

    public RegisterCluesService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RegisterCluesAsync(PrincipalEntity author, IReadOnlyCollection<RegisterClueRequest> newClueRequests)
    {
        foreach (RegisterClueRequest clueRequest in newClueRequests)
        {
            ClueAtMapStatus status = clueRequest.Found ? ClueAtMapStatus.Found : ClueAtMapStatus.NotFound;

            ClueRecordEntity? existingClue =
                await _context.ClueRecords.SingleOrDefaultAsync(c => c.MapId == clueRequest.MapId && c.ClueId == clueRequest.ClueId && c.Author == author);
            if (existingClue != null)
            {
                existingClue.UpdateStatus(status);
                continue;
            }

            ClueRecordEntity newClue = new(clueRequest.ClueId, clueRequest.MapId, author, status);
            await _context.AddAsync(newClue);
        }

        await _context.SaveChangesAsync();
    }
}
