using System.ComponentModel.DataAnnotations;

namespace DBI.Server.Features.TreasureSolver.Controllers.Requests;

/// <summary>
///     Register clues.
/// </summary>
public class RegisterCluesRequest
{
    /// <summary>
    ///     The clues to register.
    /// </summary>
    [Required]
    public required IReadOnlyCollection<RegisterClueRequest> Clues { get; init; }
}

/// <summary>
///     A clue to register.
/// </summary>
public class RegisterClueRequest
{
    /// <summary>
    ///     The unique ID of the map.
    /// </summary>
    public required long MapId { get; init; }

    /// <summary>
    ///     The unique ID of the clue.
    /// </summary>
    public required int ClueId { get; init; }

    /// <summary>
    ///     Has the clue been found in the given map, or not.
    /// </summary>
    public required bool Found { get; init; }
}
