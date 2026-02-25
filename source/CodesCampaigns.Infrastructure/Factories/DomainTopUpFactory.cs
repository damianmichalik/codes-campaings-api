using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.Entities;
using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;

namespace CodesCampaigns.Infrastructure.Factories;

public static class DomainTopUpFactory
{
    public static DomainTopUp CreateFromTopUpEntity(TopUp topUp)
        => new(
            new TopUpCode(topUp.Code),
            new Money(topUp.Amount, new CurrencyCode(topUp.Currency)),
            topUp.CampaignId.HasValue ? new CampaignId(topUp.CampaignId.Value) : null,
            topUp.CreatedAt,
            topUp.Email,
            topUp.UsedAt,
            topUp.ActiveFrom,
            topUp.ActiveTo,
            topUp.WalletExpirationDate,
            topUp.PartnerCode);
}

