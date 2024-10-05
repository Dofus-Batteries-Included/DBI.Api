namespace DBI.Server.Common.Exceptions;

class BadRequestException : ServerException
{
    public BadRequestException(string message) : base(message) { }
}
