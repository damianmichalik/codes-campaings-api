using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Repositories;
using CodesCampaigns.Infrastructure.Time;
using Hangfire;
using Hangfire.PostgreSql;
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
        
        services.AddScoped<ICampaignsRepository, CampaignsRepository>();
        services.AddScoped<ITopUpsRepository, TopUpsRepository>();
        services.AddScoped<IClock, Clock>();
        
        services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options => 
                    options.UseNpgsqlConnection(configuration.GetConnectionString("DefaultConnection"))
                ));
        
        services.AddHangfireServer();
        
        return services;
    }
}
