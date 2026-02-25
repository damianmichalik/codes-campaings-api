using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Application.Handlers;

internal sealed class UseTopUpCodeCommandHandler(
    ITopUpsRepository topUpsRepository,
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

        topUp.Use(command.Email, command.PartnerCode, clock.Current);
        await topUpsRepository.Update(topUp, cancellationToken);

        return new UseTopUpCodeResult(true);
    }
}
