namespace Server.Domains.DataCenter.Models;

/// <summary>
///     Available data types
/// </summary>
public enum RawDataType
{
    /// <summary>
    ///     French localization data
    /// </summary>
    I18NFr,

    /// <summary>
    ///     English localization data
    /// </summary>
    I18NEn,

    /// <summary>
    ///     Spanish localization data
    /// </summary>
    I18NEs,

    /// <summary>
    ///     German localization data
    /// </summary>
    I18NDe,

    /// <summary>
    ///     Portuguese localization data
    /// </summary>
    I18NPt,

    /// <summary>
    ///     Map positions data
    /// </summary>
    MapPositions,

    /// <summary>
    ///     Points of interest data
    /// </summary>
    PointOfInterest,

    /// <summary>
    ///     World graph data
    /// </summary>
    WorldGraph,

    /// <summary>
    ///     Super areas data
    /// </summary>
    SuperAreas,

    /// <summary>
    ///     Areas data
    /// </summary>
    Areas,

    /// <summary>
    ///     Sub areas data
    /// </summary>
    SubAreas
}
