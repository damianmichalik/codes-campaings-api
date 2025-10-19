using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Exceptions;
using CodesCampaigns.Application.Repositories;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class DeleteCampaignCommandHandler(ICampaignsRepository campaignsRepository) : IRequestHandler<DeleteCampaignCommand>
{
    public async Task Handle(DeleteCampaignCommand command, CancellationToken cancellationToken)
    {
        var campaign = await campaignsRepository.GetById(command.CampaignId, cancellationToken);

        if (campaign is null)
        {
            throw new CampaignNotFoundException(command.CampaignId);
        }
        
        await campaignsRepository.Delete(campaign, cancellationToken);
    }
}