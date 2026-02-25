using CodesCampaigns.Infrastructure.Entities;
using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;

namespace CodesCampaigns.Infrastructure.Factories;

public static class TopUpEntityFactory
{
    public static TopUp CreateFromDomainTopUp(DomainTopUp domainTopUp)
        => new()
        {
            Code = domainTopUp.Code,
            Amount = domainTopUp.Value.Amount,
            Currency = domainTopUp.Value.CurrencyCode.Code,
            CampaignId = domainTopUp.CampaignId?.Value,
            Email = domainTopUp.Email,
            CreatedAt = domainTopUp.CreatedAt,
            UsedAt = domainTopUp.UsedAt,
            ActiveFrom = domainTopUp.ActiveFrom,
            ActiveTo = domainTopUp.ActiveTo,
            WalletExpirationDate = domainTopUp.WalletExpirationDate,
            PartnerCode = domainTopUp.PartnerCode
        };
}

