using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Queries;

public record GetCampaignCodesQuery(CampaignId CampaignId) : IQuery<IEnumerable<TopUp>>;
