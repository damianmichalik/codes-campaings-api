using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<DomainTopUp>> GetByCampaignId(CampaignId campaignId, CancellationToken cancellationToken)
        => await context.TopUps
            .Where(c => c.CampaignId != null && c.CampaignId == campaignId.Value)
            .Select(c => new DomainTopUp
            {
                CampaignId = new CampaignId(c.CampaignId!.Value),
                Value = new Money(c.Amount, new CurrencyCode(c.Currency)),
                Code = new TopUpCode(c.Code)
            })
            .ToListAsync(cancellationToken);
}
