using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;

namespace CodesCampaigns.Application.Queries;

public record GetCampaignsQuery : IQuery<IEnumerable<Campaign>>;
