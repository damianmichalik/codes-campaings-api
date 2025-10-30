using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;

namespace CodesCampaigns.Application.Handlers;

public class CreateCampaignCommandHandler(ICampaignsRepository campaignsRepository) : ICommandHandler<CreateCampaignCommand>
{
    public async Task Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
    {
        var campaign = new Campaign(command.CampaignId, command.Name);

        await campaignsRepository.Add(campaign, cancellationToken);
    }
}
