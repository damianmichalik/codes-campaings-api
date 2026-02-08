using CodesCampaigns.Api.DTO;
using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;

namespace CodesCampaigns.Api.Factories;

public static class TopUpDtoFactory
{
    public static TopUpDto CreateFromDomainTopUp(DomainTopUp domainTopUp)
        => new()
        {
            Amount = domainTopUp.Value.Amount,
            Currency = domainTopUp.Value.CurrencyCode.Code,
            Code = domainTopUp.Code,
            CampaignId = domainTopUp.CampaignId.Value
        };
}
