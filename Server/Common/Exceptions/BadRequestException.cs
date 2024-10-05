namespace DBI.Server.Common.Exceptions;

class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
