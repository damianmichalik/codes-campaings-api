using CodesCampaigns.Application.ValueObjects;
using MediatR;

namespace CodesCampaigns.Application.Commands;

public record CreateCampaignCommand(CampaignId CampaignId, string Name) : IRequest;