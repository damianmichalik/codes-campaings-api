using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.Factories;
using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;

namespace CodesCampaigns.Infrastructure.Tests.Factories;

public class TopUpEntityFactoryTest
{
    [Fact]
    public void ItCreatesTopUpEntityFromDomainTopUp()
    {
        var code = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var campaignId = Guid.Parse("364c78e4-3553-40ee-9b7b-c3644ba101f0");
        var amount = 100.50m;
        var currency = "USD";
        var createdAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var domainTopUp = DomainTopUp.Create(
            new TopUpCode(code),
            new Money(amount, new CurrencyCode(currency)),
            new CampaignId(campaignId),
            createdAt
        );

        var actualEntity = TopUpEntityFactory.CreateFromDomainTopUp(domainTopUp);

        Assert.Equal(code, actualEntity.Code);
        Assert.Equal(amount, actualEntity.Amount);
        Assert.Equal(currency, actualEntity.Currency);
        Assert.Equal(campaignId, actualEntity.CampaignId);
        Assert.Equal(createdAt, actualEntity.CreatedAt);
        Assert.Null(actualEntity.Email);
        Assert.Null(actualEntity.UsedAt);
        Assert.Null(actualEntity.ActiveFrom);
        Assert.Null(actualEntity.ActiveTo);
        Assert.Null(actualEntity.WalletExpirationDate);
        Assert.Null(actualEntity.PartnerCode);
    }

    [Fact]
    public void ItCreatesTopUpEntityFromDomainTopUpWithAllFields()
    {
        var code = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var campaignId = Guid.Parse("364c78e4-3553-40ee-9b7b-c3644ba101f0");
        var amount = 50.00m;
        var currency = "EUR";
        var createdAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var email = "test@example.com";
        var usedAt = new DateTime(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);
        var activeFrom = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var activeTo = new DateTime(2026, 12, 31, 23, 59, 59, DateTimeKind.Utc);
        var walletExpirationDate = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var partnerCode = "PARTNER123";

        var domainTopUp = new DomainTopUp(
            new TopUpCode(code),
            new Money(amount, new CurrencyCode(currency)),
            new CampaignId(campaignId),
            createdAt,
            email,
            usedAt,
            activeFrom,
            activeTo,
            walletExpirationDate,
            partnerCode
        );

        var actualEntity = TopUpEntityFactory.CreateFromDomainTopUp(domainTopUp);

        Assert.Equal(code, actualEntity.Code);
        Assert.Equal(amount, actualEntity.Amount);
        Assert.Equal(currency, actualEntity.Currency);
        Assert.Equal(campaignId, actualEntity.CampaignId);
        Assert.Equal(createdAt, actualEntity.CreatedAt);
        Assert.Equal(email, actualEntity.Email);
        Assert.Equal(usedAt, actualEntity.UsedAt);
        Assert.Equal(activeFrom, actualEntity.ActiveFrom);
        Assert.Equal(activeTo, actualEntity.ActiveTo);
        Assert.Equal(walletExpirationDate, actualEntity.WalletExpirationDate);
        Assert.Equal(partnerCode, actualEntity.PartnerCode);
    }
}

