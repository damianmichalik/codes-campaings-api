using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Repositories;

public interface ITopUpsRepository
{
    Task Add(TopUp topUp, CancellationToken cancellationToken);
    Task AddMany(IReadOnlyCollection<TopUp> topUps, CancellationToken cancellationToken);
    Task<List<TopUp>> GetByCampaignId(CampaignId campaignId, CancellationToken cancellationToken);
    Task<TopUp?> GetByCode(TopUpCode code, CancellationToken cancellationToken);
    Task<int> CountUsedByEmailAndCampaignForMonth(string email, CampaignId campaignId, int year, int month, CancellationToken cancellationToken);
    Task<int> CountUsedByEmailAndCampaign(string email, CampaignId campaignId, CancellationToken cancellationToken);
    Task Update(TopUp topUp, CancellationToken cancellationToken);
}
