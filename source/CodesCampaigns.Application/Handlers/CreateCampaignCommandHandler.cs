using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;

namespace CodesCampaigns.Application.Handlers;

public class CreateCampaignCommandHandler(ICampaignsRepository campaignsRepository, IClock clock) : ICommandHandler<CreateCampaignCommand>
{
    public async Task Handle(CreateCampaignCommand command, CancellationToken cancellationToken)
    {
        var campaign = Campaign.Create(command.CampaignId, command.Name, clock);

        await campaignsRepository.Add(campaign, cancellationToken);
    }
}
