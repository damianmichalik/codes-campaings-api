using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.Entities;
using CodesCampaigns.Infrastructure.Factories;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Infrastructure.Tests.Factories;

public class DomainCampaignFactoryTest
{
    [Fact]
    public void ItCreatesDomainCampaignFromCampaignEntity()
    {
        var id = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var name = "Campaign Name";
        var createdDateTime = new DateTime(2026, 1, 1, 12, 00, 00);
        var updatedDateTime = new DateTime(2026, 1, 1, 13, 00, 00);

        var expectedDomainCampaign = new DomainCampaign(
            new CampaignId(id),
            name,
            createdDateTime,
            updatedDateTime
        );

        var expectedCampaignEntity = new Campaign()
        {
            Id = id,
            Name = name,
            CreatedAt = createdDateTime,
            UpdatedAt = updatedDateTime
        };

        var actualDomainCampaign = DomainCampaignFactory.CreateFromCampaignEntity(expectedCampaignEntity);
        
        Assert.Equal(expectedDomainCampaign.Id, actualDomainCampaign.Id);
        Assert.Equal(expectedDomainCampaign.Name, actualDomainCampaign.Name);
        Assert.Equal(expectedDomainCampaign.CreatedAt, actualDomainCampaign.CreatedAt);
        Assert.Equal(expectedDomainCampaign.UpdatedAt, actualDomainCampaign.UpdatedAt);
    }
}
