using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFame.Exceptions;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;

    public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception.Message, "Unhandled exception");

        var problem = new ProblemDetails();
        switch (exception)
        {
            case NotFoundException:
                problem.Title = HttpStatusCode.NotFound.ToString();
                problem.Status = (int)HttpStatusCode.NotFound;
                problem.Detail = exception.Message;
                break;
            case ValidationException:
                problem.Title = HttpStatusCode.BadRequest.ToString();
                problem.Status = (int)HttpStatusCode.BadRequest;
                problem.Detail = exception.Message;
                break;
            default:
                problem.Title = HttpStatusCode.InternalServerError.ToString();
                problem.Status = (int)HttpStatusCode.InternalServerError;
                problem.Detail = exception.Message;
                break;
        }

        httpContext.Response.StatusCode = problem!.Status!.Value;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}