using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Structured.Models.I18N;

namespace DBI.DataCenter.Structured.Models.Maps;

/// <summary>
///     A sub area in the game.
/// </summary>
public class SubArea
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
    ///     The unique ID of the containing area.
    /// </summary>
    public required long AreaId { get; init; }

    /// <summary>
    ///     The name of the containing area in all the supported languages.
    /// </summary>
    public required LocalizedText? AreaName { get; init; }

    /// <summary>
    ///     The unique ID of sub area.
    /// </summary>
    public required long SubAreaId { get; init; }

    /// <summary>
    ///     The name of the sub area in all the supported languages.
    /// </summary>
    public required LocalizedText? Name { get; init; }

    /// <summary>
    ///     The map coordinates of the center of the sub area.
    /// </summary>
    public required RawPosition Center { get; init; }

    /// <summary>
    ///     The bounds of the sub area on the map.
    /// </summary>
    public required RawBounds Bounds { get; init; }
}
