using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.ValueObjects;

namespace CodesCampaigns.Application.Repositories;

public interface ICampaignsRepository
{
    public Task Add(Campaign campaign, CancellationToken cancellationToken);
    public Task Update(Campaign campaign, CancellationToken cancellationToken);
    public Task Delete(Campaign campaign, CancellationToken cancellationToken);
    public Task<List<Campaign>> GetAll(CancellationToken cancellationToken);
    public Task<Campaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken);
}