using CodesCampaigns.Application.Entities;

namespace CodesCampaigns.Application.Repositories;

public interface ITopUpsRepository
{
    Task Add(TopUp topUp, CancellationToken cancellationToken);
    Task AddMany(IReadOnlyCollection<TopUp> topUps, CancellationToken cancellationToken);
}
