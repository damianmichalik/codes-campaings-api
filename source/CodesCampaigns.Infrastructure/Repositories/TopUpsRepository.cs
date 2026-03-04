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
            .Where(c => c.CampaignId == campaignId.Value)
            .ToListAsync(cancellationToken);

        return entities
            .Select(DomainTopUpFactory.CreateFromTopUpEntity)
            .ToList();
    }

    public async Task<DomainTopUp?> GetByCode(TopUpCode code, CancellationToken cancellationToken)
    {
        var entity = await context.TopUps
            .FirstOrDefaultAsync(t => t.Code == code.Value, cancellationToken);

        return entity is null ? null : DomainTopUpFactory.CreateFromTopUpEntity(entity);
    }

    public Task<int> CountUsedByEmailAndCampaign(string email, CampaignId campaignId, CancellationToken cancellationToken)
        => context.TopUps
            .Where(t => t.CampaignId == campaignId.Value
                && t.UsedAt.HasValue
                && t.Email != null
                && EF.Functions.ILike(t.Email, email))
            .CountAsync(cancellationToken);

    public Task<int> CountUsedByEmailAndCampaignForMonth(
        string email, CampaignId campaignId, int year, int month, CancellationToken cancellationToken)
        => context.TopUps
            .Where(t => t.CampaignId == campaignId.Value
                && t.UsedAt.HasValue
                && t.UsedAt.Value.Year == year
                && t.UsedAt.Value.Month == month
                && t.Email != null
                && EF.Functions.ILike(t.Email, email))
            .CountAsync(cancellationToken);

    public async Task Update(DomainTopUp topUp, CancellationToken cancellationToken)
    {
        var entity = await context.TopUps
            .FirstOrDefaultAsync(t => t.Code == topUp.Code.Value, cancellationToken);

        if (entity is null)
        {
            return;
        }

        entity.Email = topUp.Email;
        entity.UsedAt = topUp.UsedAt;
        entity.PartnerCode = topUp.PartnerCode;
        entity.UpdatedAt = topUp.UsedAt;

        await context.SaveChangesAsync(cancellationToken);
    }
}
