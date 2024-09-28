using System.ComponentModel.DataAnnotations;
using Server.Common.Models;

namespace Server.Features.TreasureSolver.Models;

public class Clue
{
    [Required]
    public required int ClueId { get; init; }

    [Required]
    public LocalizedText Name { get; init; } = new();
}
