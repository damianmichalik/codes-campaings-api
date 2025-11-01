namespace CodesCampaigns.Api.DTO;

public class TopUpDto
{
    public required Guid Code { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public Guid? CampaignId { get; set; }
}
