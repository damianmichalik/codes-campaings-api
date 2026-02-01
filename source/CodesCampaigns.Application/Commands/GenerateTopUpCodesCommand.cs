using CodesCampaigns.Application.Abstractions;

namespace CodesCampaigns.Application.Commands;

public record GenerateTopUpCodesCommand(Guid CampaignId, int Count, decimal Value, string Currency, DateTime DateTime) : ICommand;
