using CodesCampaigns.Api.Factories;
using CodesCampaigns.Domain.ValueObjects;
using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;

namespace CodesCampaigns.Api.Tests.Factories;

public class TopUpDtoFactoryTest
{
    [Fact]
    public void ItCreatesTopUpDtoFromDomainTopUp()
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

        var actualDto = TopUpDtoFactory.CreateFromDomainTopUp(domainTopUp);

        Assert.Equal(code, actualDto.Code);
        Assert.Equal(amount, actualDto.Amount);
        Assert.Equal(currency, actualDto.Currency);
        Assert.Equal(campaignId, actualDto.CampaignId);
    }

    [Fact]
    public void ItCreatesTopUpDtoFromDomainTopUpWithAllFields()
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

        var actualDto = TopUpDtoFactory.CreateFromDomainTopUp(domainTopUp);

        Assert.Equal(code, actualDto.Code);
        Assert.Equal(amount, actualDto.Amount);
        Assert.Equal(currency, actualDto.Currency);
        Assert.Equal(campaignId, actualDto.CampaignId);
    }
}

