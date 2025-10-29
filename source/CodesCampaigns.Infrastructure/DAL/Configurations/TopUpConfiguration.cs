using CodesCampaigns.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodesCampaigns.Infrastructure.DAL.Configurations;

public class TopUpConfiguration : IEntityTypeConfiguration<TopUp>
{
    public void Configure(EntityTypeBuilder<TopUp> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne<Campaign>().WithMany(c => c.TopUps).HasForeignKey(x => x.CampaignId);
    }
}
