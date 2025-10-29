namespace CodesCampaigns.Infrastructure.Entities;

public class TopUp
{
    public int Id {  get; set; }
    public required Guid Code { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public Guid? CampaignId { get; set; }
}
