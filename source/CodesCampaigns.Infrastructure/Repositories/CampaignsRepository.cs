using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.Repositories;
using CodesCampaigns.Application.ValueObjects;
using CodesCampaigns.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.Repositories;

public class CampaignsRepository(AppDbContext context) : ICampaignsRepository
{
    public async Task Add(Campaign campaign, CancellationToken cancellationToken)
    {
        context.Campaigns.Add(campaign);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(Campaign campaign, CancellationToken cancellationToken)
    {
        context.Campaigns.Update(campaign);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Campaign campaign, CancellationToken cancellationToken)
    {
        context.Campaigns.Remove(campaign);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Campaign>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Campaigns
            .ToListAsync(cancellationToken);
    }

    public async Task<Campaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken)
    {
        return await context.Campaigns
            .Where(c => c.Id == campaignId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}