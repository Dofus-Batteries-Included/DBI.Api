namespace DBI.DataCenter.Exceptions;

public class DataCenterException : Exception
{
    public DataCenterException(string? message, Exception? innerException = null) : base(message ?? "An unexpected error has occured.", innerException) { }
}
