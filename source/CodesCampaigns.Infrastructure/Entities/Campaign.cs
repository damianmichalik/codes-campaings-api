namespace CodesCampaigns.Infrastructure.Entities;

public class Campaign
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IReadOnlyCollection<TopUp> TopUps { get; private set; } = [];
}
