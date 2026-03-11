using System.ComponentModel.DataAnnotations;
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
    private static readonly EmailAddressAttribute EmailValidator = new();

    public async Task<UseTopUpCodeResult> Handle(UseTopUpCodeCommand command, CancellationToken cancellationToken)
    {
        if (!EmailValidator.IsValid(command.Email))
        {
            return new UseTopUpCodeResult(false, "invalid_data");
        }

        var code = new TopUpCode(command.Code);
        var topUp = await topUpsRepository.GetByCode(code, cancellationToken);

        if (topUp is null || topUp.UsedAt.HasValue || topUp.ActiveFrom > clock.Current)
        {
            return new UseTopUpCodeResult(false, "invalid_data");
        }

        if (topUp.ActiveTo.HasValue && topUp.ActiveTo <= clock.Current)
        {
            return new UseTopUpCodeResult(false, "code_expired");
        }

        var campaign = await campaignsRepository.GetById(topUp.CampaignId, cancellationToken);
        if (campaign is not null)
        {
            var totalUsageCount = await topUpsRepository.CountUsedByEmailAndCampaign(
                command.Email, topUp.CampaignId, cancellationToken);

            if (totalUsageCount >= campaign.MaxNumberOfTopUpsPerUser)
            {
                return new UseTopUpCodeResult(false, "usage_limit_exceeded_in_campaign");
            }
        }

        topUp.Use(command.Email, command.PartnerCode, clock.Current);
        await topUpsRepository.Update(topUp, cancellationToken);

        return new UseTopUpCodeResult(true);
    }
}
