using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DBI.Server.Common.Exceptions;

class ExceptionHandler : IExceptionHandler
{
    readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public bool ShowException { get; set; }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = statusCode;
        ProblemDetails problemDetails = new()
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            Title = "An error occurred while processing your request.",
            Status = statusCode,
            Detail = exception.Message
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
}
