using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Repositories;

public interface ITopUpsRepository
{
    Task Add(TopUp topUp, CancellationToken cancellationToken);
    Task AddMany(IReadOnlyCollection<TopUp> topUps, CancellationToken cancellationToken);
    Task<List<TopUp>> GetByCampaignId(CampaignId campaignId, CancellationToken cancellationToken);
}
