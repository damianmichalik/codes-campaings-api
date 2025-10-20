using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Application.Repositories;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class GetCampaignsQueryHandler(ICampaignsRepository campaignsRepository) : IRequestHandler<GetCampaignsQuery, IEnumerable<Campaign>>
{
    public async Task<IEnumerable<Campaign>> Handle(GetCampaignsQuery query, CancellationToken cancellationToken)
        => await campaignsRepository.GetAll(cancellationToken);
}
