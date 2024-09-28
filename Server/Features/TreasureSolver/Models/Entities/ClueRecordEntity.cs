using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Features.Identity.Models.Entities;

namespace Server.Features.TreasureSolver.Models.Entities;

public class ClueRecordEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // for EFC
    ClueRecordEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public ClueRecordEntity(int clueId, long mapId, PrincipalEntity author, ClueAtMapStatus status = ClueAtMapStatus.Found)
    {
        Author = author;
        LastModificationDate = DateTime.Now.ToUniversalTime();
        ClueId = clueId;
        MapId = mapId;
        Status = status;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Guid { get; private set; }

    public PrincipalEntity Author { get; private set; }
    public DateTime LastModificationDate { get; private set; }
    public int ClueId { get; private set; }
    public long MapId { get; private set; }
    public ClueAtMapStatus Status { get; private set; }

    public void UpdateStatus(ClueAtMapStatus newStatus)
    {
        Status = newStatus;
        LastModificationDate = DateTime.Now.ToUniversalTime();
    }
}

public enum ClueAtMapStatus
{
    Found = 1,
    NotFound = 2
}
