using System.Text.Json;

namespace DBI.DataCenter.Serialization;

/// <remarks>
///     Custom naming policy because System.Text.Json v7 doesn't have this built-in yet, and System.Text.Json v8 doesn't work with BepInEx
///     (the framework used by the DBI.Plugins project that consumes this library).
/// </remarks>
class KebabCaseNamingPolicy : JsonNamingPolicy
{
    public static KebabCaseNamingPolicy Instance { get; } = new();
    public override string ConvertName(string name) => string.Concat(name.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
}
