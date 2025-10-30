using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Entities;

public class TopUp
{
    public required TopUpCode Code { get; set; }
    public required Money Value { get; set; }
    public required CampaignId CampaignId { get; set; }
}
