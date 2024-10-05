namespace DBI.Server.Common.Exceptions;

class NotFoundException : ServerException
{
    public NotFoundException(string? message) : base(message ?? "The requested data could not be found.") { }
}
