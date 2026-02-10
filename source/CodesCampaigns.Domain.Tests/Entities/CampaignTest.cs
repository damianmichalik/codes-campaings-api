using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.Entities;

public class CampaignTest
{
    private sealed class FakeClock(DateTime current) : IClock
    {
        public DateTime Current { get; } = current;
    }

    private static readonly CampaignId ValidId = CampaignId.Create();
    private static readonly DateTime FixedTime = new(2024, 1, 1, 12, 0, 0);
    private static readonly IClock Clock = new FakeClock(FixedTime);

    [Fact]
    public void ItCreatesCampaignWithValidData()
    {
        var campaign = Campaign.Create(ValidId, "Summer Campaign", Clock);

        Assert.Equal(ValidId, campaign.Id);
        Assert.Equal("Summer Campaign", campaign.Name);
        Assert.Equal(FixedTime, campaign.CreatedAt);
        Assert.Null(campaign.UpdatedAt);
        Assert.Empty(campaign.TopUps);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ItThrowsExceptionWhenCreatingWithInvalidName(string name)
    {
        var ex = Assert.Throws<ArgumentException>(() => Campaign.Create(ValidId, name, Clock));

        Assert.Equal("Campaign name cannot be empty. (Parameter 'name')", ex.Message);
    }

    [Fact]
    public void ItUpdatesCampaignName()
    {
        var campaign = Campaign.Create(ValidId, "Old Name", Clock);
        var updateTime = new DateTime(2024, 6, 1, 9, 0, 0);
        var updateClock = new FakeClock(updateTime);

        campaign.Update("New Name", updateClock);

        Assert.Equal("New Name", campaign.Name);
        Assert.Equal(updateTime, campaign.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ItThrowsExceptionWhenUpdatingWithInvalidName(string name)
    {
        var campaign = Campaign.Create(ValidId, "Valid Name", Clock);

        var ex = Assert.Throws<ArgumentException>(() => campaign.Update(name, Clock));

        Assert.Equal("Campaign name cannot be empty. (Parameter 'name')", ex.Message);
    }

    [Fact]
    public void ItDoesNotChangeCreatedAtOnUpdate()
    {
        var campaign = Campaign.Create(ValidId, "Original", Clock);
        var updateClock = new FakeClock(FixedTime.AddDays(1));

        campaign.Update("Updated", updateClock);

        Assert.Equal(FixedTime, campaign.CreatedAt);
    }
}