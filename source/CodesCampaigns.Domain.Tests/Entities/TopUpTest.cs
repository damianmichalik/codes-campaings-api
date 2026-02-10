using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.Entities;

public class TopUpTest
{
    private static readonly TopUpCode ValidCode = TopUpCode.Create();
    private static readonly Money ValidMoney = new(10m, new CurrencyCode("PLN"));
    private static readonly CampaignId ValidCampaignId = CampaignId.Create();
    private static readonly DateTime FixedTime = new(2024, 1, 1, 12, 0, 0);

    [Fact]
    public void ItCreatesTopUpWithRequiredFields()
    {
        var topUp = TopUp.Create(ValidCode, ValidMoney, ValidCampaignId, FixedTime);

        Assert.Equal(ValidCode, topUp.Code);
        Assert.Equal(ValidMoney, topUp.Value);
        Assert.Equal(ValidCampaignId, topUp.CampaignId);
        Assert.Equal(FixedTime, topUp.CreatedAt);
    }

    [Fact]
    public void ItSetsOptionalFieldsToNullOnCreate()
    {
        var topUp = TopUp.Create(ValidCode, ValidMoney, ValidCampaignId, FixedTime);

        Assert.Null(topUp.Email);
        Assert.Null(topUp.UsedAt);
        Assert.Null(topUp.ActiveFrom);
        Assert.Null(topUp.ActiveTo);
        Assert.Null(topUp.WalletExpirationDate);
        Assert.Null(topUp.PartnerCode);
    }
}