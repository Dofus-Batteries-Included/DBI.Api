namespace DBI.DataCenter.Exceptions;

public class DataNotFoundException : DataCenterException
{
    public DataNotFoundException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }
}
