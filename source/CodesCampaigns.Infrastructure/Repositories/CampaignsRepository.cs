using CodesCampaigns.Application.Exceptions;
using DomainCampaign = CodesCampaigns.Application.Entities.Campaign;
using CodesCampaigns.Application.Repositories;
using CodesCampaigns.Application.ValueObjects;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.Repositories;

public class CampaignsRepository(AppDbContext context) : ICampaignsRepository
{
    public async Task Add(DomainCampaign campaign, CancellationToken cancellationToken)
    {
        var campaignEntity = new Campaign
        {
            Name = campaign.Name
        };
        context.Campaigns.Add(campaignEntity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(DomainCampaign campaign, CancellationToken cancellationToken)
    {
        var campaignEntity = await context.Campaigns.FindAsync(
            new object[] { campaign.Id.Value }, cancellationToken
        );
        
        if (campaignEntity is null)
        {
            throw new CampaignNotFoundException(campaign.Id);
        }
        
        campaignEntity.Name = campaign.Name;
        
        context.Campaigns.Update(campaignEntity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(DomainCampaign campaign, CancellationToken cancellationToken)
        => await context.Campaigns
            .Where(c => c.Id == campaign.Id.Value)
            .ExecuteDeleteAsync(cancellationToken);

    public async Task<List<DomainCampaign>> GetAll(CancellationToken cancellationToken)
        => await context.Campaigns
            .Select(c => new DomainCampaign(new CampaignId(c.Id), c.Name))
            .ToListAsync(cancellationToken);

    public async Task<DomainCampaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken)
    {
        var campainEntity = await context.Campaigns
            .Where(c => c.Id == campaignId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        return campainEntity is null ? null : new DomainCampaign(new CampaignId(campainEntity.Id), campainEntity.Name);
    }
}
