using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Entities;

public class Campaign
{
    public CampaignId Id { get; }
    public string Name { get; private set; }
    public int MaxNumberOfTopUpsPerUser { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<TopUp> TopUps { get; } = [];

    internal Campaign(CampaignId id, string name, DateTime createdAt, DateTime? updatedAt, int maxNumberOfTopUpsPerUser = 3)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        MaxNumberOfTopUpsPerUser = maxNumberOfTopUpsPerUser;
    }

    public void Update(string name, IClock clock)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Campaign name cannot be empty.", nameof(name));
        }

        Name = name;
        UpdatedAt = clock.Current;
    }

    public static Campaign Create(
        CampaignId id,
        string name,
        IClock clock,
        int maxNumberOfTopUpsPerUser = 3)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Campaign name cannot be empty.", nameof(name));
        }

        return new Campaign(id, name, clock.Current, null, maxNumberOfTopUpsPerUser);
    }

}
