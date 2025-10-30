using CodesCampaigns.Domain.Entities;

namespace CodesCampaigns.Domain.Repositories;

public interface ITopUpsRepository
{
    Task Add(TopUp topUp, CancellationToken cancellationToken);
    Task AddMany(IReadOnlyCollection<TopUp> topUps, CancellationToken cancellationToken);
}
