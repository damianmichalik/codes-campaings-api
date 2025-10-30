using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Application.Queries;

namespace CodesCampaigns.Application.Handlers;

public class GetCampaignsQueryHandler(ICampaignsRepository campaignsRepository) : IQueryHandler<GetCampaignsQuery, IEnumerable<Campaign>>
{
    public async Task<IEnumerable<Campaign>> Handle(GetCampaignsQuery query, CancellationToken cancellationToken)
        => await campaignsRepository.GetAll(cancellationToken);
}
