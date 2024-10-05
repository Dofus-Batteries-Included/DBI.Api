using DBI.Server.Features.Identity.Models.Entities;
using DBI.Server.Features.TreasureSolver.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBI.Server.Infrastructure.Database;

/// <summary>
///     Main db context
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     The principals that have registered.
    ///     Principals have an API key that allows them to authenticate API calls.
    ///     <br />
    ///     This entity is used to track who submitted clues in the database.
    /// </summary>
    public DbSet<PrincipalEntity> Principals { get; private set; }

    /// <summary>
    ///     The clues that have been submitted by the community. Each clue is associated with a <see cref="PrincipalEntity" />.
    /// </summary>
    public DbSet<ClueRecordEntity> ClueRecords { get; private set; }
}
