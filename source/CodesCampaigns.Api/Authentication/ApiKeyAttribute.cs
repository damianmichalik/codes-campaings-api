using Microsoft.AspNetCore.Mvc;

namespace CodesCampaigns.Api.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ApiKeyAttribute : TypeFilterAttribute
{
    public ApiKeyAttribute() : base(typeof(ApiKeyFilter))
    {
    }
}
