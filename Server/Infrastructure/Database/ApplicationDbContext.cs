using Microsoft.EntityFrameworkCore;
using Server.Features.Identity.Models.Entities;
using Server.Features.TreasureSolver.Models.Entities;

namespace Server.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<PrincipalEntity> Principals { get; private set; }
    public DbSet<ClueRecordEntity> ClueRecords { get; private set; }
}
