using Microsoft.EntityFrameworkCore;
using Server.Domains.Identity.Models.Entities;
using Server.Domains.TreasureSolver.Models.Entities;

namespace Server.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<PrincipalEntity> Principals { get; private set; }
    public DbSet<ClueRecordEntity> ClueRecords { get; private set; }
}
