using CodesCampaigns.Api.Factories;
using CodesCampaigns.Domain.ValueObjects;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;

namespace CodesCampaigns.Api.Tests.Factories;

public class CampaignDtoFactoryTest
{
    [Fact]
    public void ItCreatesCampaignDtoFromDomainCampaign()
    {
        var id = Guid.Parse("264c78e4-3553-40ee-9b7b-c3644ba101f0");
        var name = "Campaign Name";
        var createdAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        var domainCampaign = new DomainCampaign(
            new CampaignId(id),
            name,
            createdAt,
            null
        );

        var actualDto = CampaignDtoFactory.CreateFromDomainCampaign(domainCampaign);

        Assert.Equal(id.ToString(), actualDto.Id);
        Assert.Equal(name, actualDto.Name);
    }

    [Fact]
    public void ItCreatesCampaignDtoFromDomainCampaignWithUpdatedAt()
    {
        var id = Guid.Parse("364c78e4-3553-40ee-9b7b-c3644ba101f0");
        var name = "Updated Campaign";
        var createdAt = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var updatedAt = new DateTime(2026, 1, 2, 14, 0, 0, DateTimeKind.Utc);

        var domainCampaign = new DomainCampaign(
            new CampaignId(id),
            name,
            createdAt,
            updatedAt
        );

        var actualDto = CampaignDtoFactory.CreateFromDomainCampaign(domainCampaign);

        Assert.Equal(id.ToString(), actualDto.Id);
        Assert.Equal(name, actualDto.Name);
    }
}

