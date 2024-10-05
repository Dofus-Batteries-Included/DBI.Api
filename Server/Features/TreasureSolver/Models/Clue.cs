using System.ComponentModel.DataAnnotations;
using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.Server.Features.TreasureSolver.Models;

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
    public LocalizedText? Name { get; init; } = new();
}
