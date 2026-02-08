using DomainTopUp = CodesCampaigns.Domain.Entities.TopUp;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.Repositories;

public class TopUpsRepository(AppDbContext context) : ITopUpsRepository
{
    public async Task Add(DomainTopUp topUp, CancellationToken cancellationToken)
    {
        context.TopUps.Add(TopUpEntityFactory.CreateFromDomainTopUp(topUp));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddMany(IReadOnlyCollection<DomainTopUp> topUps, CancellationToken cancellationToken)
    {
        foreach (var topUp in topUps)
        {
            context.TopUps.Add(TopUpEntityFactory.CreateFromDomainTopUp(topUp));
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<DomainTopUp>> GetByCampaignId(CampaignId campaignId, CancellationToken cancellationToken)
    {
        var entities = await context.TopUps
            .Where(c => c.CampaignId != null && c.CampaignId == campaignId.Value)
            .ToListAsync(cancellationToken);

        return entities
            .Select(DomainTopUpFactory.CreateFromTopUpEntity)
            .ToList();
    }
}
