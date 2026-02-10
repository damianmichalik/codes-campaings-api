namespace CodesCampaigns.Domain.ValueObjects;

public record CurrencyCode
{
    public string Code { get; }

    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Currency code cannot be empty");
        }

        if (value.Length != 3)
        {
            throw new ArgumentException("Currency code must be exactly 3 letters (ISO 4217)");
        }

        Code = value.ToUpperInvariant();
    }
    
    public override string ToString() => Code;
}
