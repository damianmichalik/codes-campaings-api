using CodesCampaigns.Domain.Abstractions;

namespace CodesCampaigns.Infrastructure.Time;

public class Clock : IClock
{
    public DateTime Current => DateTime.UtcNow;
}
