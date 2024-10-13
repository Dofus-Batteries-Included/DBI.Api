using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Raw;

public class RawDataJsonOptionsProvider
{
    readonly JsonSerializerOptions _camelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    readonly JsonSerializerOptions _kebabCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.KebabCaseLower, PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(JsonNamingPolicy.KebabCaseLower) }
    };

    /// <inheritdoc cref="GetJsonSerializerOptions(string,DBI.DataCenter.Raw.Models.RawDataType)" />
    /// <remarks>
    ///     Settings for old versions, kept for retro-compatibility.
    /// </remarks>
    public JsonSerializerOptions GetJsonSerializerOptions(string gameVersion, RawDataType rawDataType) =>
        rawDataType switch
        {
            RawDataType.Maps or RawDataType.WorldGraph => _kebabCaseOptions,
            _ => _camelCaseOptions
        };

    /// <summary>
    ///     Return the <see cref="JsonSerializerOptions" /> to use to read the given data from files extracted by the <see cref="ddcVersion" /> of DDC
    ///     from the <see cref="gameVersion" /> version of the game.
    /// </summary>
    public JsonSerializerOptions GetJsonSerializerOptions(string ddcVersion, string gameVersion, RawDataType rawDataType)
    {
        if (!Version.TryParse(ddcVersion, out Version? versionParsed) || versionParsed < new Version(0, 9))
        {
            GetJsonSerializerOptions(gameVersion, rawDataType);
        }

        return _kebabCaseOptions;
    }
}
