﻿using DBI.Server.Features.TreasureSolver.Models;

namespace DBI.Server.Features.TreasureSolver.Services.Clues.DataSources;

class StaticClueRecordsSource : IClueRecordsSource
{
    public StaticClueRecordsSource(IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> clues, DateTime? lastModificationDate = null)
    {
        Clues = clues;
        LastModificationDate = lastModificationDate;
    }

    public DateTime? LastModificationDate { get; }
    public IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>> Clues { get; }

    public Task<DateTime?> GetLastModificationDate(CancellationToken cancellationToken = default) => Task.FromResult(LastModificationDate);
    public Task<IReadOnlyCollection<ClueRecord>> GetCluesInMap(long mapId, CancellationToken cancellationToken = default) => Task.FromResult(Clues.GetValueOrDefault(mapId) ?? []);
    public Task<IReadOnlyDictionary<long, IReadOnlyCollection<ClueRecord>>> ExportData(CancellationToken cancellationToken = default) => Task.FromResult(Clues);
}
