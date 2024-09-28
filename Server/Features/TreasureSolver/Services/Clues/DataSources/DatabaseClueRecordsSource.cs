using Microsoft.EntityFrameworkCore;
using Server.Features.TreasureSolver.Models;
using Server.Features.TreasureSolver.Models.Entities;
using Server.Infrastructure.Database;

namespace Server.Features.TreasureSolver.Services.Clues.DataSources;

class DatabaseClueRecordsSource : IClueRecordsSource
{
    readonly ApplicationDbContext _context;

    public DatabaseClueRecordsSource(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DateTime?> GetLastModificationDate() =>
        await _context.ClueRecords.DefaultIfEmpty().MaxAsync<ClueRecordEntity?, DateTime?>(c => c == null ? null : c.LastModificationDate);

    public async Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId) =>
        (await _context.ClueRecords.Where(c => c.MapId == mapId).GroupBy(c => c.ClueId).Select(g => g.OrderByDescending(c => c.LastModificationDate).First()).ToArrayAsync())
        .Select(
            c => new ClueRecord
            {
                RecordDate = c.LastModificationDate,
                MapId = c.MapId,
                ClueId = c.ClueId,
                Found = c.Status == ClueAtMapStatus.Found
            }
        )
        .ToArray();

    public async Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData() =>
        (await _context.ClueRecords.GroupBy(c => c.ClueId).Select(g => g.OrderByDescending(c => c.LastModificationDate).First()).ToArrayAsync()).GroupBy(c => c.MapId)
        .ToDictionary(
            g => g.Key,
            IReadOnlyCollection<ClueRecord> (g) => g.Select(
                    c => new ClueRecord
                    {
                        RecordDate = c.LastModificationDate,
                        MapId = c.MapId,
                        ClueId = c.ClueId,
                        Found = c.Status == ClueAtMapStatus.Found
                    }
                )
                .ToArray()
        );
}
