using System.Globalization;

namespace CodesCampaigns.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }
    public CurrencyCode CurrencyCode { get; }

    public Money(decimal amount, CurrencyCode currencyCode)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        Amount = amount;
        CurrencyCode = currencyCode ?? throw new ArgumentNullException(nameof(currencyCode));
    }

    public override string ToString() => $"{CurrencyCode} {Amount.ToString("N2", CultureInfo.InvariantCulture)}";
}
