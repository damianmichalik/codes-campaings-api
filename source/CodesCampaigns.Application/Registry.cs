using CodesCampaigns.Application.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace CodesCampaigns.Application;

public static class Registry
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
        => services.AddScoped<GenerateTopUpBatchJob>();
}
