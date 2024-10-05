using DBI.Server.Common.Models;

namespace DBI.Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A map in the game.
/// </summary>
public class Map
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
    ///     The unique ID of the containing area, if any.
    /// </summary>
    public required long? AreaId { get; init; }

    /// <summary>
    ///     The name of the containing area in all the supported languages.
    /// </summary>
    public required LocalizedText? AreaName { get; init; }

    /// <summary>
    ///     The unique ID of the containing sub area, if any.
    /// </summary>
    public required long SubAreaId { get; init; }

    /// <summary>
    ///     The name of the containing sub area in all the supported languages.
    /// </summary>
    public required LocalizedText? SubAreaName { get; init; }

    /// <summary>
    ///     The unique ID of the map.
    /// </summary>
    public required long MapId { get; init; }

    /// <summary>
    ///     The name of the map in all the supported languages.
    /// </summary>
    public required LocalizedText? MapName { get; init; }

    /// <summary>
    ///     The coordinates of the map.
    /// </summary>
    public required Position Position { get; init; }

    /// <summary>
    ///     The number of cells in the map.
    /// </summary>
    public required int CellsCount { get; init; }
}
