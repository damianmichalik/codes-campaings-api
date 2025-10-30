using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Repositories;

public interface ICampaignsRepository
{
    Task Add(Campaign campaign, CancellationToken cancellationToken);
    Task Update(Campaign campaign, CancellationToken cancellationToken);
    Task Delete(Campaign campaign, CancellationToken cancellationToken);
    Task<List<Campaign>> GetAll(CancellationToken cancellationToken);
    Task<Campaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken);
}
