using CodesCampaigns.Application.ValueObjects;
using MediatR;

namespace CodesCampaigns.Application.Commands;

public record UpdateCampaignCommand(CampaignId CampaignId, string Name) : IRequest;