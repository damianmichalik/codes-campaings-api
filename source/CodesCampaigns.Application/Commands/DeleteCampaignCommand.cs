using CodesCampaigns.Application.ValueObjects;
using MediatR;

namespace CodesCampaigns.Application.Commands;

public record DeleteCampaignCommand(CampaignId CampaignId) : IRequest;