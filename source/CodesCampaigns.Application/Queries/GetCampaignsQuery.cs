using CodesCampaigns.Application.Entities;
using MediatR;

namespace CodesCampaigns.Application.Queries;

public record GetCampaignsQuery : IRequest<IEnumerable<Campaign>>;