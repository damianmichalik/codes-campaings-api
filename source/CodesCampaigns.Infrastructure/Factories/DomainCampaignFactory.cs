using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.Entities;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Infrastructure.Factories;

public static class DomainCampaignFactory
{
    public static DomainCampaign CreateFromCampaignEntity(Campaign campaign)
        => new(
            new CampaignId(campaign.Id),
            campaign.Name,
            campaign.CreatedAt,
            campaign.UpdatedAt
        );
}
