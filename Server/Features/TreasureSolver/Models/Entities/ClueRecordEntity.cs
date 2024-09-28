using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Features.Identity.Models.Entities;

namespace Server.Features.TreasureSolver.Models.Entities;

/// <summary>
///     A clue record in the database.
/// </summary>
public class ClueRecordEntity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // for EFC
    ClueRecordEntity() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// </summary>
    public ClueRecordEntity(int clueId, long mapId, PrincipalEntity author, ClueAtMapStatus status = ClueAtMapStatus.Found)
    {
        Author = author;
        LastModificationDate = DateTime.Now.ToUniversalTime();
        ClueId = clueId;
        MapId = mapId;
        Status = status;
    }

    /// <summary>
    ///     The unique ID of the clue record.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Guid { get; private set; }

    /// <summary>
    ///     The principal that has created the record.
    /// </summary>
    public PrincipalEntity Author { get; private set; }

    /// <summary>
    ///     The last time the record was modified.
    /// </summary>
    /// <remarks>
    ///     The same principal cannot save two record with the same ClueId and MapId.
    ///     They should update the existing record instead with the new status of the clue.
    ///     When they do that, this date is updated too.
    /// </remarks>
    /// <seealso cref="UpdateStatus" />
    public DateTime LastModificationDate { get; private set; }

    /// <summary>
    ///     The unique ID of the clue that has been found (or not found).
    /// </summary>
    public int ClueId { get; private set; }

    /// <summary>
    ///     The unique ID of the map where the clue has been found (or not found).
    /// </summary>
    public long MapId { get; private set; }

    /// <summary>
    ///     The status of the clue in the map, has it been found or not found last time it was checked.
    /// </summary>
    public ClueAtMapStatus Status { get; private set; }

    /// <summary>
    ///     Update the status of the clue in the map.
    /// </summary>
    /// <remarks>
    ///     Updating the status of the clue will update the <see cref="LastModificationDate" />, even if the status itself doesn't change.
    /// </remarks>
    /// <seealso cref="LastModificationDate" />
    public void UpdateStatus(ClueAtMapStatus newStatus)
    {
        Status = newStatus;
        LastModificationDate = DateTime.Now.ToUniversalTime();
    }
}

/// <summary>
///     Status of a clue in a map.
/// </summary>
public enum ClueAtMapStatus
{
    /// <summary>
    ///     The clue has been found in the map.
    /// </summary>
    Found = 1,

    /// <summary>
    ///     The clue has not been found in the map.
    /// </summary>
    NotFound = 2
}
