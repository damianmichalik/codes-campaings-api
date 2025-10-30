using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Commands;

public record DeleteCampaignCommand(CampaignId CampaignId) : ICommand;
