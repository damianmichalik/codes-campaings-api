using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Jobs;
using Microsoft.Extensions.DependencyInjection;

namespace CodesCampaigns.Application;

public static class Registry
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        services.AddScoped<GenerateTopUpBatchJob>();
        services.Scan(scan => scan.FromAssembliesOf(typeof(CreateCampaignCommandHandler))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}
