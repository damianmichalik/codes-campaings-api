using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Repositories;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class CreateCampaignCommandHandler(ICampaignsRepository campaignsRepository) : IRequestHandler<CreateCampaignCommand>
{
    public async Task Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
    {
        var campaign = new Campaign(command.CampaignId, command.Name);

        await campaignsRepository.Add(campaign, cancellationToken);
    }
}