using CodesCampaigns.Application.ValueObjects;

namespace CodesCampaigns.Application.Entities;

public class Campaign
{
    public CampaignId Id { get; set; } = null!; 
    public string Name { get; set; } = string.Empty;

    private Campaign()
    {
    }

    public Campaign(CampaignId campaignId, string name)
    {
        Id = campaignId;
        Name = name;
    }
}