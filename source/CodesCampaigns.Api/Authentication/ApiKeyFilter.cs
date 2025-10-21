using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CodesCampaigns.Api.Authentication;

public class ApiKeyFilter : IAsyncActionFilter
{
    private readonly AuthenticationSettings _settings;
    private const string APIKEYNAME = "X-API-KEY";

    public ApiKeyFilter(IOptions<AuthenticationSettings> options)
        => _settings = options.Value;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Result = new ContentResult
            {
                StatusCode = 401,
                Content = "API Key was not provided."
            };
            return;
        }

        if (!_settings.ApiKey.Equals(extractedApiKey, StringComparison.Ordinal))
        {
            context.Result = new ContentResult
            {
                StatusCode = 403,
                Content = "Unauthorized client."
            };
            return;
        }

        await next();
    }
}
