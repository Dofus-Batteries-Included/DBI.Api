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

    public JsonSerializerOptions GetJsonSerializerOptions(string version, RawDataType rawDataType)
    {
        if (Version.TryParse(version, out Version? versionParsed) && versionParsed < new Version(0, 9))
        {
            // old behavior
            return rawDataType switch
            {
                RawDataType.Maps or RawDataType.WorldGraph => _kebabCaseOptions,
                _ => _camelCaseOptions
            };
        }

        return _kebabCaseOptions;
    }
}
