using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Queries;

public record GetCampaignQuery(CampaignId CampaignId) : IQuery<Campaign>;
