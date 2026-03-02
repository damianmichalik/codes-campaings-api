namespace CodesCampaigns.Infrastructure.Entities;

public class Campaign : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int MaxNumberOfTopUpsPerUser { get; set; } = 3;
    public IReadOnlyCollection<TopUp> TopUps { get; private set; } = new List<TopUp>();
}
