using CodesCampaigns.Domain.Exceptions;
using DomainCampaign = CodesCampaigns.Domain.Entities.Campaign;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using CodesCampaigns.Infrastructure.DAL;
using CodesCampaigns.Infrastructure.Factories;
using Microsoft.EntityFrameworkCore;

namespace CodesCampaigns.Infrastructure.Repositories;

public class CampaignsRepository(AppDbContext context) : ICampaignsRepository
{
    public async Task Add(DomainCampaign campaign, CancellationToken cancellationToken)
    {
        context.Campaigns.Add(CampaignEntityFactory.CreateFromDomainCampaign(campaign));
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
        
        context.Campaigns.Update(CampaignEntityFactory.UpdateFromDomainCampaign(campaignEntity, campaign));
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(DomainCampaign campaign, CancellationToken cancellationToken)
        => await context.Campaigns
            .Where(c => c.Id == campaign.Id.Value)
            .ExecuteDeleteAsync(cancellationToken);

    public async Task<List<DomainCampaign>> GetAll(CancellationToken cancellationToken)
        => await context.Campaigns
            .Select(campainEntity => DomainCampaignFactory.CreateFromCampaignEntity(campainEntity))
            .ToListAsync(cancellationToken);

    public async Task<DomainCampaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken)
    {
        var campainEntity = await context.Campaigns
            .Where(c => c.Id == campaignId.Value)
            .FirstOrDefaultAsync(cancellationToken);

        return campainEntity is null ? null : DomainCampaignFactory.CreateFromCampaignEntity(campainEntity);
    }
}
