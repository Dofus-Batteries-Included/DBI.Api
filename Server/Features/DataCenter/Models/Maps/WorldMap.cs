using DBI.Server.Common.Models;

namespace DBI.Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A world map in the game.
/// </summary>
public class WorldMap
{
    /// <summary>
    ///     The unique ID of the world map.
    /// </summary>
    public required int WorldMapId { get; init; }

    /// <summary>
    ///     The name of the world map in all the supported languages.
    /// </summary>
    public required LocalizedText? Name { get; init; }

    /// <summary>
    ///     The coordinates of the top left corner of the map.
    /// </summary>
    public required Position Origin { get; init; }
}
