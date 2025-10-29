namespace CodesCampaigns.Application.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public CurrencyCode CurrencyCode { get; }

    private Money() => CurrencyCode = null!;

    public Money(decimal amount, CurrencyCode currencyCode)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        }

        Amount = amount;
        CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
    }

    public override string ToString() => $"{CurrencyCode} {Amount:N2}";
}
