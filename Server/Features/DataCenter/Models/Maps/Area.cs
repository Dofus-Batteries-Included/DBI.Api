using DBI.Server.Common.Models;

namespace DBI.Server.Features.DataCenter.Models.Maps;

/// <summary>
///     An area in the game.
/// </summary>
public class Area
{
    /// <summary>
    ///     The unique ID of the containing world map, if any.
    /// </summary>
    public required int? WorldMapId { get; init; }

    /// <summary>
    ///     The name of the containing world map in all the supported languages.
    /// </summary>
    public required LocalizedText? WorldMapName { get; init; }

    /// <summary>
    ///     The unique ID of the containing super area, if any.
    /// </summary>
    public required long? SuperAreaId { get; init; }

    /// <summary>
    ///     The name of the containing super area in all the supported languages.
    /// </summary>
    public required LocalizedText? SuperAreaName { get; init; }

    /// <summary>
    ///     The unique ID of the area.
    /// </summary>
    public required long AreaId { get; init; }

    /// <summary>
    ///     The name of the area in all the supported languages.
    /// </summary>
    public required LocalizedText? Name { get; init; }

    /// <summary>
    ///     The bounds of the area on the map.
    /// </summary>
    public required Bounds Bounds { get; init; }
}
