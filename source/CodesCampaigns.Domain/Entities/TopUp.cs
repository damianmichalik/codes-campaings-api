using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Entities;

public class TopUp
{
    public TopUpCode Code { get; }
    public Money Value { get; }
    public CampaignId CampaignId { get; }
    public string? Email { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? UsedAt { get; private set; }
    public DateTime? ActiveFrom { get; }
    public DateTime? ActiveTo { get; }
    public DateTime? WalletExpirationDate { get; }
    public string? PartnerCode { get; }

    internal TopUp(
        TopUpCode code,
        Money value,
        CampaignId campaignId,
        DateTime createdAt,
        string? email = null,
        DateTime? usedAt = null,
        DateTime? activeFrom = null,
        DateTime? activeTo = null,
        DateTime? walletExpirationDate = null,
        string? partnerCode = null)
    {
        Code = code;
        Value = value;
        CampaignId = campaignId;
        CreatedAt = createdAt;
        Email = email;
        UsedAt = usedAt;
        ActiveFrom = activeFrom;
        ActiveTo = activeTo;
        WalletExpirationDate = walletExpirationDate;
        PartnerCode = partnerCode;
    }

    public static TopUp Create(
        TopUpCode code,
        Money value,
        CampaignId campaignId,
        DateTime createdAt) => new(code, value, campaignId, createdAt);
}
