using System.ComponentModel.DataAnnotations;
using Server.Common.Models;
using Server.Domains.DataCenter.Models;

namespace Server.Domains.TreasureSolver.Controllers.Responses;

public class FindNextPositionResponse
{
    [Required]
    public required bool Found { get; init; }

    public required Position? MapPosition { get; init; }
}
