using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Repositories;
using CodesCampaigns.Application.ValueObjects;

namespace CodesCampaigns.Application.Jobs;

public class GenerateTopUpBatchJob(ITopUpsRepository topUpsRepository)
{
    public async Task GenerateBatch(GenerateTopUpCodesCommand command, int count, CancellationToken cancellationToken)
    {
        var topUps = new List<TopUp>([]);
        for (var i = 0; i < count; i++)
        {
            var topUp = new TopUp
            {
                Code = TopUpCode.Create(),
                Value = new Money(command.Value, new CurrencyCode(command.Currency)),
                CampaignId = command.CampaignId
            };
            topUps.Add(topUp);
        }
        await topUpsRepository.AddMany(topUps.AsReadOnly(), cancellationToken);
    }
}
