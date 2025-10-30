using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Commands;

public record UpdateCampaignCommand(CampaignId CampaignId, string Name) : ICommand;
