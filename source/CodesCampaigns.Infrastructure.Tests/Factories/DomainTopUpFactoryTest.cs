using CodesCampaigns.Infrastructure.Factories;
using TopUpEntity = CodesCampaigns.Infrastructure.Entities.TopUp;

namespace CodesCampaigns.Infrastructure.Tests.Factories;

public class DomainTopUpFactoryTest
{
    [Fact]
    public void ItCreatesDomainTopUpFromTopUpEntity()
    {
        var code = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var campaignId = Guid.Parse("364c78e4-3553-40ee-9b7b-c3644ba101f0");
        var amount = 100.50m;
        var currency = "USD";
        var createdAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var topUpEntity = new TopUpEntity
        {
            Code = code,
            Amount = amount,
            Currency = currency,
            CampaignId = campaignId,
            CreatedAt = createdAt
        };

        var actualDomainTopUp = DomainTopUpFactory.CreateFromTopUpEntity(topUpEntity);

        Assert.Equal(code, actualDomainTopUp.Code.Value);
        Assert.Equal(amount, actualDomainTopUp.Value.Amount);
        Assert.Equal(currency, actualDomainTopUp.Value.CurrencyCode.Code);
        Assert.Equal(campaignId, actualDomainTopUp.CampaignId.Value);
        Assert.Equal(createdAt, actualDomainTopUp.CreatedAt);
        Assert.Null(actualDomainTopUp.Email);
        Assert.Null(actualDomainTopUp.UsedAt);
        Assert.Null(actualDomainTopUp.ActiveFrom);
        Assert.Null(actualDomainTopUp.ActiveTo);
        Assert.Null(actualDomainTopUp.WalletExpirationDate);
        Assert.Null(actualDomainTopUp.PartnerCode);
    }

    [Fact]
    public void ItCreatesDomainTopUpFromTopUpEntityWithAllFields()
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

        var topUpEntity = new TopUpEntity
        {
            Code = code,
            Amount = amount,
            Currency = currency,
            CampaignId = campaignId,
            CreatedAt = createdAt,
            Email = email,
            UsedAt = usedAt,
            ActiveFrom = activeFrom,
            ActiveTo = activeTo,
            WalletExpirationDate = walletExpirationDate,
            PartnerCode = partnerCode
        };

        var actualDomainTopUp = DomainTopUpFactory.CreateFromTopUpEntity(topUpEntity);

        Assert.Equal(code, actualDomainTopUp.Code.Value);
        Assert.Equal(amount, actualDomainTopUp.Value.Amount);
        Assert.Equal(currency, actualDomainTopUp.Value.CurrencyCode.Code);
        Assert.Equal(campaignId, actualDomainTopUp.CampaignId.Value);
        Assert.Equal(createdAt, actualDomainTopUp.CreatedAt);
        Assert.Equal(email, actualDomainTopUp.Email);
        Assert.Equal(usedAt, actualDomainTopUp.UsedAt);
        Assert.Equal(activeFrom, actualDomainTopUp.ActiveFrom);
        Assert.Equal(activeTo, actualDomainTopUp.ActiveTo);
        Assert.Equal(walletExpirationDate, actualDomainTopUp.WalletExpirationDate);
        Assert.Equal(partnerCode, actualDomainTopUp.PartnerCode);
    }
}

