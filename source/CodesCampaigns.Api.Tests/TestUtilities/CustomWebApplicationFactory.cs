using CodesCampaigns.Infrastructure.DAL;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodesCampaigns.Api.Tests.TestUtilities;

internal sealed class CustomWebApplicationFactory(string connectionString) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));
            
            var hangfireDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(IGlobalConfiguration));
            if (hangfireDescriptor != null)
            {
                services.Remove(hangfireDescriptor);
            }

            var jobClientDescriptor = services.FirstOrDefault(
                d => d.ServiceType == typeof(IBackgroundJobClient));
            if (jobClientDescriptor != null)
            {
                services.Remove(jobClientDescriptor);
            }

            services.AddHangfire(config => config.UseMemoryStorage());
            services.AddHangfireServer();
        });

        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        return host;
    }
}
