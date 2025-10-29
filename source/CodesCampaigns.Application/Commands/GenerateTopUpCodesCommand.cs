using MediatR;

namespace CodesCampaigns.Application.Commands;

public record GenerateTopUpCodesCommand(Guid CampaignId, int Count, decimal Value, string Currency) : IRequest;
