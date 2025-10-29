using CodesCampaigns.Application.ValueObjects;

namespace CodesCampaigns.Application.Entities;

public class Campaign
{
    public CampaignId Id { get; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<TopUp> TopUps { get; private set; } = [];

    public Campaign(CampaignId id, string name)
    {
        Id = id;
        Name = name;
    }
}
