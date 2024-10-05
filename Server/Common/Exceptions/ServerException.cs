namespace DBI.Server.Common.Exceptions;

class ServerException : Exception
{
    public ServerException(string? message, Exception? exception = null) : base(message ?? "An unexpected error has occured.", exception) { }
}
