using System.ComponentModel.DataAnnotations;

namespace Server.Features.TreasureSolver.Models;

public class Map
{
    [Required]
    public required long MapId { get; init; }

    [Required]
    public required int PosX { get; init; }

    [Required]
    public required int PosY { get; init; }

    [Required]
    public int WorldMap { get; set; }
}
