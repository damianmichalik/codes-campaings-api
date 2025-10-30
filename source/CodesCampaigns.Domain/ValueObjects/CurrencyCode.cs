namespace CodesCampaigns.Domain.ValueObjects;

public record CurrencyCode
{
    public string Code { get; }
    
    private CurrencyCode() => Code = null!;

    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Currency code cannot be empty.", nameof(value));
        }

        if (value.Length != 3)
        {
            throw new ArgumentException("Currency code must be exactly 3 letters (ISO 4217).", nameof(value));
        }

        Code = value.ToUpperInvariant();
    }
    
    public override string ToString() => Code!;
}
