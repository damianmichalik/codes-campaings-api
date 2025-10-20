using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Repositories;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodesCampaigns.Infrastructure;

public static class Registry
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCampaignCommandHandler).Assembly)
        );
        
        services.AddScoped<ICampaignsRepository, CampaignsRepository>();
        
        return services;
    }
}
