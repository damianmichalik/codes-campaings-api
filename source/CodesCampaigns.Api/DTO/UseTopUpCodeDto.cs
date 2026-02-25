using System.ComponentModel.DataAnnotations;

namespace CodesCampaigns.Api.DTO;

public class UseTopUpCodeDto
{
    [Required]
    public required string PartnerCode { get; set; }

    [Required]
    public required Guid Code { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
