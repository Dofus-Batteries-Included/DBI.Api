using System.Text.Json;
using System.Text.Json.Serialization;
using DBI.DataCenter.Raw.Models;
using DBI.DataCenter.Serialization;

namespace DBI.DataCenter.Raw;

public class RawDataJsonOptionsProvider
{
    readonly JsonSerializerOptions _preZeroNineDefaultOptions = new() { PropertyNameCaseInsensitive = true };

    readonly JsonSerializerOptions _preZeroNineKebabCaseOptions = new()
    {
        PropertyNamingPolicy = KebabCaseNamingPolicy.Instance, PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter(KebabCaseNamingPolicy.Instance) }
    };

    readonly JsonSerializerOptions _postZeroNineKebabCaseOptions = new()
    {
        PropertyNamingPolicy = KebabCaseNamingPolicy.Instance,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(KebabCaseNamingPolicy.Instance) },
        NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals
    };

    /// <inheritdoc cref="GetJsonSerializerOptions(string,DBI.DataCenter.Raw.Models.RawDataType)" />
    /// <remarks>
    ///     Settings for old versions, kept for retro-compatibility.
    /// </remarks>
    public JsonSerializerOptions GetJsonSerializerOptions(string gameVersion, RawDataType rawDataType) =>
        rawDataType switch
        {
            RawDataType.Maps or RawDataType.WorldGraph => _preZeroNineKebabCaseOptions,
            _ => _preZeroNineDefaultOptions
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

        return _postZeroNineKebabCaseOptions;
    }
}
