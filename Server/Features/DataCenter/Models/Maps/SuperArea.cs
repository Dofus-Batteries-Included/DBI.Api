using Server.Common.Models;

namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A super area in the game.
/// </summary>
public class SuperArea
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
    ///     The unique ID of the super area.
    /// </summary>
    public required long SuperAreaId { get; init; }

    /// <summary>
    ///     The name of the super area in all the supported languages.
    /// </summary>
    public required LocalizedText? Name { get; init; }
}
