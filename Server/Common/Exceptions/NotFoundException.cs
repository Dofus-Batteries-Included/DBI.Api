namespace DBI.Server.Common.Exceptions;

class NotFoundException : ServerException
{
    public NotFoundException(string message) : base(message) { }
}
