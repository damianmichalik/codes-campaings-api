

using System.ComponentModel.DataAnnotations;

namespace CodesCampaigns.Infrastructure.Entities;

public class TopUp : BaseEntity
{
    public int Id {  get; set; }
    public required Guid Code { get; set; }
    public required decimal Amount { get; set; }
    [MaxLength(3)]
    public required string Currency { get; set; }
    public Guid? CampaignId { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    public DateTime? UsedAt { get; set; }
    public DateTime? ActiveFrom { get; set; }
    public DateTime? ActiveTo { get; set; }
    public DateTime? WalletExpirationDate { get; set; }
    [MaxLength(100)]
    public string? PartnerCode { get; set; }
}
