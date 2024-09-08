using System.ComponentModel.DataAnnotations;

namespace Server.Domains.TreasureSolver.Models;

public class Clue
{
    [Required]
    public required int ClueId { get; init; }

    [Required]
    public LocalizedText Name { get; init; } = new();
}
