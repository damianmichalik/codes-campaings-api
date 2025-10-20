using CodesCampaigns.Application.Entities;
using CodesCampaigns.Application.ValueObjects;

namespace CodesCampaigns.Application.Repositories;

public interface ICampaignsRepository
{
    Task Add(Campaign campaign, CancellationToken cancellationToken);
    Task Update(Campaign campaign, CancellationToken cancellationToken);
    Task Delete(Campaign campaign, CancellationToken cancellationToken);
    Task<List<Campaign>> GetAll(CancellationToken cancellationToken);
    Task<Campaign?> GetById(CampaignId campaignId, CancellationToken cancellationToken);
}
