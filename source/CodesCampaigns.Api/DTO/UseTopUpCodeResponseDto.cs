using System.Text.Json.Serialization;

namespace CodesCampaigns.Api.DTO;

public class UseTopUpCodeResponseDto
{
    public required bool Success { get; set; }

    [JsonPropertyName("error_code")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ErrorCode { get; set; }
}
