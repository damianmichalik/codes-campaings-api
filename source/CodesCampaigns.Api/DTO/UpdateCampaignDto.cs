using System.ComponentModel.DataAnnotations;

namespace CodesCampaigns.Api.DTO;

public class UpdateCampaignDto
{
    [Required] 
    public required string Name { get; set; } = string.Empty;
}
