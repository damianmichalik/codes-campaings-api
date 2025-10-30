using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Repositories;

namespace CodesCampaigns.Application.Handlers;

public class GetCampaignQueryHandler(ICampaignsRepository campaignsRepository) : IQueryHandler<GetCampaignQuery, Campaign>
{
    public async Task<Campaign> Handle(GetCampaignQuery query, CancellationToken cancellationToken)
    {
        var campaign = await campaignsRepository.GetById(query.CampaignId, cancellationToken);

        if (campaign is null)
        {
            throw new CampaignNotFoundException(query.CampaignId);
        }

        return campaign;
    }
}
