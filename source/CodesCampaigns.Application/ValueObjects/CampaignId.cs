using CodesCampaigns.Application.Exceptions;

namespace CodesCampaigns.Application.ValueObjects;

public sealed record CampaignId
{
    public Guid Value { get; }

    public CampaignId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException(value);
        }
        
        Value = value;
    }
    
    private CampaignId() { }
    
    public static CampaignId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(CampaignId campaignId)
        => campaignId.Value;
    
    public static implicit operator CampaignId(Guid value)
        => new(value);
    
    public override string ToString() => Value.ToString();
}