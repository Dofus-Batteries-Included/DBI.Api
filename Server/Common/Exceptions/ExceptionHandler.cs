using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DBI.Server.Common.Exceptions;

class ExceptionHandler : IExceptionHandler
{
    const string DefaultTitle = "An error occurred while processing your request.";
    readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public bool ShowException { get; set; }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (HttpStatusCode statusCode, string title, string detail) = HandleException(exception);

        httpContext.Response.StatusCode = (int)statusCode;
        ProblemDetails problemDetails = new()
        {
            Type = GetStatusCodeType(statusCode),
            Title = title,
            Status = (int)statusCode,
            Detail = detail
        };

        if (await _problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = ShowException ? exception : null
                }
            ))
        {
            return true;
        }

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }

    static (HttpStatusCode statusCode, string title, string detail) HandleException(Exception exn)
    {
        switch (exn)
        {
            case ServerException serverExn:
                return HandleServerException(serverExn);
            default:
                return (HttpStatusCode.InternalServerError, DefaultTitle, exn.Message);
        }
    }

    static (HttpStatusCode statusCode, string title, string detail) HandleServerException(ServerException exn)
    {
        HttpStatusCode statusCode = exn switch
        {
            BadRequestException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };

        return (statusCode, DefaultTitle, exn.Message);
    }
    static string GetStatusCodeType(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.BadRequest => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            HttpStatusCode.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2",
            HttpStatusCode.PaymentRequired => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.3",
            HttpStatusCode.Forbidden => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4",
            HttpStatusCode.NotFound => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            HttpStatusCode.MethodNotAllowed => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.6",
            HttpStatusCode.NotAcceptable => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.7",
            HttpStatusCode.ProxyAuthenticationRequired => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.8",
            HttpStatusCode.RequestTimeout => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.9",
            HttpStatusCode.Conflict => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
            HttpStatusCode.Gone => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.11",
            HttpStatusCode.LengthRequired => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.12",
            HttpStatusCode.PreconditionFailed => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.13",
            HttpStatusCode.RequestEntityTooLarge => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.14",
            HttpStatusCode.RequestUriTooLong => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.15",
            HttpStatusCode.UnsupportedMediaType => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.16",
            HttpStatusCode.RequestedRangeNotSatisfiable => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.17",
            HttpStatusCode.ExpectationFailed => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.18",
            HttpStatusCode.MisdirectedRequest => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.20",
            HttpStatusCode.UnprocessableContent => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21",
            HttpStatusCode.UpgradeRequired => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.22",
            HttpStatusCode.InternalServerError => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
            HttpStatusCode.NotImplemented => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2",
            HttpStatusCode.BadGateway => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.3",
            HttpStatusCode.ServiceUnavailable => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.4",
            HttpStatusCode.GatewayTimeout => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.5",
            HttpStatusCode.HttpVersionNotSupported => "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.6",
            _ => "https://datatracker.ietf.org/doc/html/rfc9110#section-15"
        };
}
