using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.Repositories;

namespace CodesCampaigns.Application.Handlers;

public class DeleteCampaignCommandHandler(ICampaignsRepository campaignsRepository) : ICommandHandler<DeleteCampaignCommand>
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
