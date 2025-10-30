using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Commands;

public record CreateCampaignCommand(CampaignId CampaignId, string Name) : ICommand;
