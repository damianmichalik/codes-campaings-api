using CodesCampaigns.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CodesCampaigns.Api.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionMessage = exception.Message;
        logger.LogError(
            "Error Message: {exceptionMessage}, Time of occurrence {time}",
            exceptionMessage, DateTime.UtcNow);

        httpContext.Response.StatusCode = exception switch
        {
            CampaignNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "An error occured",
                Detail = exception.Message,
            });

        return true;
    }
}