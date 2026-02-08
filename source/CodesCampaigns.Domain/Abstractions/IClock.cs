namespace CodesCampaigns.Domain.Abstractions;

public interface IClock
{
    DateTime Current { get; }
}
