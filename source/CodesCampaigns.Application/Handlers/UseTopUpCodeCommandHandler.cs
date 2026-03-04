using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Handlers;

internal sealed class UseTopUpCodeCommandHandler(
    ITopUpsRepository topUpsRepository,
    ICampaignsRepository campaignsRepository,
    IClock clock) : ICommandHandler<UseTopUpCodeCommand, UseTopUpCodeResult>
{
    public async Task<UseTopUpCodeResult> Handle(UseTopUpCodeCommand command, CancellationToken cancellationToken)
    {
        var code = new TopUpCode(command.Code);
        var topUp = await topUpsRepository.GetByCode(code, cancellationToken);

        if (topUp is null || topUp.UsedAt.HasValue)
        {
            return new UseTopUpCodeResult(false, "invalid_data");
        }

        var campaign = await campaignsRepository.GetById(topUp.CampaignId, cancellationToken);
        if (campaign is not null)
        {
            var usageCount = await topUpsRepository.CountUsedByEmailAndCampaignForMonth(
                command.Email, topUp.CampaignId, clock.Current.Year, clock.Current.Month, cancellationToken);

            if (usageCount >= campaign.MaxNumberOfTopUpsPerUser)
            {
                return new UseTopUpCodeResult(false, "usage_limit_exceeded");
            }
        }

        topUp.Use(command.Email, command.PartnerCode, clock.Current);
        await topUpsRepository.Update(topUp, cancellationToken);

        return new UseTopUpCodeResult(true);
    }
}
