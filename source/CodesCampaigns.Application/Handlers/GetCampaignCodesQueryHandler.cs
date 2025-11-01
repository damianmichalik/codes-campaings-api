using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Repositories;

namespace CodesCampaigns.Application.Handlers;

public class GetCampaignCodesQueryHandler(ITopUpsRepository topUpsRepository) : IQueryHandler<GetCampaignCodesQuery, IEnumerable<TopUp>>
{
    public async Task<IEnumerable<TopUp>> Handle(GetCampaignCodesQuery query, CancellationToken cancellationToken)
        => await topUpsRepository.GetByCampaignId(query.CampaignId, cancellationToken);
}
