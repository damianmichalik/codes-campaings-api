using CodesCampaigns.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Campaign> Campaigns { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}