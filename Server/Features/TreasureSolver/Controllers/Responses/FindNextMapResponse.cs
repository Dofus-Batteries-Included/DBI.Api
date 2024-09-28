using System.ComponentModel.DataAnnotations;
using Server.Common.Models;

namespace Server.Features.TreasureSolver.Controllers.Responses;

public class FindNextMapResponse
{
    [Required]
    public required bool Found { get; init; }

    public required long? MapId { get; init; }
    public required Position? MapPosition { get; init; }
}
