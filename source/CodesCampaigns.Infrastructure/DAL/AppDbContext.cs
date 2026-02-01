using CodesCampaigns.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.DAL;

public class AppDbContext(
    DbContextOptions<AppDbContext> options
) : DbContext(options) {
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<TopUp> TopUps { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
}
