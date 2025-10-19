using CodesCampaigns.Api.Tests.TestUtilities;
using CodesCampaigns.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using Testcontainers.PostgreSql;

namespace CodesCampaigns.Api.Tests.Hooks;

[Binding]
public class FeatureHooks
{
    public static PostgreSqlContainer? PgContainer;
    public static CustomWebApplicationFactory? Factory;

    [BeforeFeature]
    public static async Task BeforeFeature()
    {
        // Start container only once per feature
        PgContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();

        await PgContainer.StartAsync();

        // Create API factory connected to container
        Factory = new CustomWebApplicationFactory(PgContainer.GetConnectionString());

        // Apply migrations and optionally seed default data
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    [AfterFeature]
    public static async Task AfterFeature()
    {
        if (Factory != null) Factory.Dispose();
        if (PgContainer != null) await PgContainer.DisposeAsync();
    }
    
    [AfterScenario]
    public void AfterScenario()
    {
        using var scope = Factory!.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Remove all campaigns (or any other tables you want to reset)
        db.Campaigns.RemoveRange(db.Campaigns);
        db.SaveChanges();
    }
}