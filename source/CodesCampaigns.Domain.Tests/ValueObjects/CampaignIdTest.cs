using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.ValueObjects;

public class CampaignIdTest
{
    [Fact]
    public void ItCreatesCampaignIdFromValidGuid()
    {
        var guid = Guid.NewGuid();
        var campaignId = new CampaignId(guid);

        Assert.Equal(guid, campaignId.Value);
    }

    [Fact]
    public void ItThrowsExceptionWhenGuidIsEmpty()
    {
        var ex = Assert.Throws<InvalidEntityIdException>(() => new CampaignId(Guid.Empty));

        Assert.Equal($"Cannot set: {Guid.Empty} as entity identifier.", ex.Message);
    }

    [Fact]
    public void ItCreatesUniqueCampaignIdViaFactoryMethod()
    {
        var first = CampaignId.Create();
        var second = CampaignId.Create();

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void ItConvertsImplicitlyToGuid()
    {
        var guid = Guid.NewGuid();
        var campaignId = new CampaignId(guid);

        Guid result = campaignId;

        Assert.Equal(guid, result);
    }

    [Fact]
    public void ItConvertsImplicitlyFromGuid()
    {
        var guid = Guid.NewGuid();

        CampaignId campaignId = guid;

        Assert.Equal(guid, campaignId.Value);
    }

    [Fact]
    public void ItReturnsGuidStringFromToString()
    {
        var guid = Guid.NewGuid();
        var campaignId = new CampaignId(guid);

        Assert.Equal(guid.ToString(), campaignId.ToString());
    }

    [Fact]
    public void ItIsEqualToAnotherCampaignIdWithSameGuid()
    {
        var guid = Guid.NewGuid();
        var first = new CampaignId(guid);
        var second = new CampaignId(guid);

        Assert.Equal(first, second);
    }
}