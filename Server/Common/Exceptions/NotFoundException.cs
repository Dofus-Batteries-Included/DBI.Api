namespace DBI.Server.Common.Exceptions;

class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}
