using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Exceptions;

public static class ErrorMessages
{
    public static string VersionNotFound(string version) => $"Could not find data for version {version}.";
    public static string RawDataNotFound(RawDataType type) => $"Could not find data for {type}.";
    public static string RawDataNotFound(RawDataType type, string version) => $"Could not find data for {type} in version {version}.";
}
