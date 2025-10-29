using DomainTopUp = CodesCampaigns.Application.Entities.TopUp;
using CodesCampaigns.Application.Repositories;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Entities;

namespace CodesCampaigns.Infrastructure.Repositories;

public class TopUpsRepository(AppDbContext context) : ITopUpsRepository
{
    public async Task Add(DomainTopUp topUp, CancellationToken cancellationToken)
    {
        var topUpEntity = new TopUp
        {
            Amount = topUp.Value.Amount,
            Currency = topUp.Value.CurrencyCode.Code,
            Code = topUp.Code,
            CampaignId = topUp.CampaignId,
        };
        context.TopUps.Add(topUpEntity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddMany(IReadOnlyCollection<DomainTopUp> topUps, CancellationToken cancellationToken)
    {
        foreach (var topUp in topUps)
        {
            var topUpEntity = new TopUp
            {
                Amount = topUp.Value.Amount,
                Currency = topUp.Value.CurrencyCode.Code,
                Code = topUp.Code,
                CampaignId = topUp.CampaignId,
            };
            context.TopUps.Add(topUpEntity);
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
