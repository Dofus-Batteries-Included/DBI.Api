using System.ComponentModel.DataAnnotations;
using Server.Domains.TreasureSolver.Services.Maps;

namespace Server.Domains.TreasureSolver.Controllers.Responses;

public class FindNextPositionResponse
{
    [Required]
    public required bool Found { get; init; }

    public required Position? MapPosition { get; init; }
}
