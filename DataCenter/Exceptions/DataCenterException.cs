using DBI.DataCenter.Raw.Models;

namespace DBI.DataCenter.Exceptions;

public class DataCenterException : Exception
{
    public DataCenterException(string? message, Exception? innerException = null) : base(message ?? "An unexpected error occured.", innerException)
    {
    }
}

public class RawDataFileNotFound : DataCenterException
{
    public RawDataFileNotFound(RawDataType type, Exception? innerException = null) : base($"Could not find data for {type}.", innerException)
    {
    }

    public RawDataFileNotFound(RawDataType type, string version, Exception? innerException = null) : base($"Could not find data for {type} in version {version}.", innerException)
    {
    }
}
