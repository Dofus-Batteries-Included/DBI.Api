namespace DBI.DataCenter.Raw;

/// <summary>
///     Raw data file
/// </summary>
public interface IRawDataFile
{
    /// <summary>
    ///     The name of the file
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     The version of the game that corresponds to the data file
    /// </summary>
    string GameVersion { get; }

    /// <summary>
    ///     The version of the DDC application that extracted the data
    /// </summary>
    string? DdcVersion { get; }

    /// <summary>
    ///     The content type of the data
    /// </summary>
    string ContentType { get; }

    /// <summary>
    ///     Get a stream to read data from the file
    /// </summary>
    Stream OpenRead();
}
