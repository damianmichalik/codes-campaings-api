using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.Entities;
using CodesCampaigns.Infrastructure.Factories;
using CodesCampaigns.Infrastructure.Time;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Infrastructure.Tests.Factories;

public class CampaignEntityFactoryTest
{
    [Fact]
    public void ItCreatesCampaignEntityFromDomainCampaign()
    {
        var id = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var name = "Campaign Name";
        var dateTime = new DateTime(2026, 1, 1, 12, 00, 00);
        var clock = new FakeClock
        {
            Current = dateTime
        };

        var domainCampaign = DomainCampaign.Create(
            new CampaignId(id),
            name,
            clock
        );

        var expectedCampaignEntity = new Campaign()
        {
            Id = id,
            Name = name,
            CreatedAt = dateTime
        };

        var actualCampaignEntity = CampaignEntityFactory.CreateFromDomainCampaign(domainCampaign);
        
        Assert.Equal(expectedCampaignEntity.Id, actualCampaignEntity.Id);
        Assert.Equal(expectedCampaignEntity.Name, actualCampaignEntity.Name);
        Assert.Equal(expectedCampaignEntity.CreatedAt, actualCampaignEntity.CreatedAt);
        Assert.Equal(expectedCampaignEntity.UpdatedAt, actualCampaignEntity.UpdatedAt);
    }
    
    [Fact]
    public void ItUpdatesCampaignEntityFromDomainCampaign()
    {
        var id = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var name = "Campaign Name";
        var updatedName = "Updated campaing name";
        var createdDateTime = new DateTime(2026, 1, 1, 12, 00, 00);
        var updatedDateTime = new DateTime(2026, 1, 1, 13, 00, 00);

        var domainCampaign = new DomainCampaign(
            new CampaignId(id),
            updatedName,
            createdDateTime,
            updatedDateTime
        );

        var campaignEntity = new Campaign()
        {
            Id = id,
            Name = name,
            CreatedAt = createdDateTime
        };

        var expectedCampaignEntity = new Campaign()
        {
            Id = id,
            Name = updatedName,
            CreatedAt = createdDateTime,
            UpdatedAt = updatedDateTime
        };

        var actualCampaignEntity = CampaignEntityFactory.UpdateFromDomainCampaign(
            campaignEntity, 
            domainCampaign
        );
        
        Assert.Equal(expectedCampaignEntity.Id, actualCampaignEntity.Id);
        Assert.Equal(expectedCampaignEntity.Name, actualCampaignEntity.Name);
        Assert.Equal(expectedCampaignEntity.CreatedAt, actualCampaignEntity.CreatedAt);
        Assert.Equal(expectedCampaignEntity.UpdatedAt, actualCampaignEntity.UpdatedAt);
    }
}
