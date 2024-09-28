using System.ComponentModel.DataAnnotations;
using Server.Common.Models;

namespace Server.Features.TreasureSolver.Models;

/// <summary>
///     A clue.
/// </summary>
public class Clue
{
    /// <summary>
    ///     The unique ID of the clue.
    /// </summary>
    [Required]
    public required int ClueId { get; init; }

    /// <summary>
    ///     The name of the clue, in all the supported languages.
    /// </summary>
    [Required]
    public LocalizedText Name { get; init; } = new();
}
