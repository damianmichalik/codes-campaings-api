using CodesCampaigns.Api.DTO;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Api.Factories;

public static class CampaignDtoFactory
{
    public static CampaignDto CreateFromDomainCampaign(DomainCampaign domainCampaign)
        => new()
        {
            Name = domainCampaign.Name,
            Id = domainCampaign.Id.ToString()
        };
}
