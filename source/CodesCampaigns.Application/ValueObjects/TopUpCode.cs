using CodesCampaigns.Application.Exceptions;

namespace CodesCampaigns.Application.ValueObjects;

public sealed record TopUpCode
{
    public Guid Value { get; set; }

    public TopUpCode(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidEntityIdException(value);
        }
        
        Value = value;
    }
    
    public static TopUpCode Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(TopUpCode topUpCode)
        => topUpCode.Value;
    
    public static implicit operator TopUpCode(Guid value)
        => new(value);
    
    public override string ToString() => Value.ToString();
}
