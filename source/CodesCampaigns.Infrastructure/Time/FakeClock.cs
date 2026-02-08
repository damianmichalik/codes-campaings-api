using CodesCampaigns.Domain.Abstractions;

namespace CodesCampaigns.Infrastructure.Time;

public class FakeClock : IClock
{
    public DateTime Current { get; set; }
}
