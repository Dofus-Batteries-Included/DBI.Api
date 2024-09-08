using System.ComponentModel.DataAnnotations;

namespace Server.Domains.TreasureSolver.Controllers.Requests;

public class RegisterCluesRequest
{
    [Required]
    public required IReadOnlyCollection<RegisterClueRequest> Clues { get; init; }
}

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
