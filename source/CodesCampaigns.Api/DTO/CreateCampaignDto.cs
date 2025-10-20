using System.ComponentModel.DataAnnotations;

namespace CodesCampaigns.Api.DTO;

public class CreateCampaignDto
{
    [Required]
    public required string Name { get; set; } = string.Empty;
}
