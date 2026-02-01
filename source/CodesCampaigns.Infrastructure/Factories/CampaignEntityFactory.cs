using CodesCampaigns.Infrastructure.Entities;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Infrastructure.Factories;

public static class CampaignEntityFactory
{
    public static Campaign CreateFromDomainCampaign(DomainCampaign domainCampaign)
        => new()
        {
            Name = domainCampaign.Name,
            Id = domainCampaign.Id,
            CreatedAt = domainCampaign.CreatedAt,
            UpdatedAt = domainCampaign.UpdatedAt
        };
    public static Campaign UpdateFromDomainCampaign(Campaign campaign, DomainCampaign domainCampaign)
    {
        campaign.Name = domainCampaign.Name;
        campaign.UpdatedAt = domainCampaign.UpdatedAt;
        return campaign;
    }
}
