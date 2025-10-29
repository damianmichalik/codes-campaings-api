using System.ComponentModel.DataAnnotations;

namespace CodesCampaigns.Api.DTO;

public class GenerateTopUpCodesDto
{
    [Required]
    public required int Count { get; set; } = 1;
    [Required]
    public required decimal Value { get; set; }
    [Required]
    public required string Currency { get; set; }
}
