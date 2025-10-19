using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Exceptions;
using CodesCampaigns.Application.Repositories;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class UpdateCampaignCommandHandler(ICampaignsRepository campaignsRepository) : IRequestHandler<UpdateCampaignCommand>
{
    public async Task Handle(UpdateCampaignCommand command, CancellationToken cancellationToken)
    {
        var campaign = await campaignsRepository.GetById(command.CampaignId, cancellationToken);

        if (campaign is null)
        {
            throw new CampaignNotFoundException(command.CampaignId);
        }

        campaign.Name = command.Name;

        await campaignsRepository.Update(campaign, cancellationToken);
    }
}