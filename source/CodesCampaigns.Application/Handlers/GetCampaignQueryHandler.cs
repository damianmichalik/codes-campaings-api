using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Exceptions;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Application.Repositories;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class GetCampaignQueryHandler(ICampaignsRepository campaignsRepository) : IRequestHandler<GetCampaignQuery, Campaign>
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