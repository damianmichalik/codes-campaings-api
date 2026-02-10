using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.ValueObjects;

public class MoneyTest
{
    [Fact]
    public void ItCreatesMoneyObject()
    {
        var amount = 100m;
        var currencyCode = new CurrencyCode("PLN");

        var money = new Money(amount, currencyCode);
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currencyCode, money.CurrencyCode);
    }
    
    [Fact]
    public void ItThrowsExceptionWhenAmountIsNegative()
    {
        var amount = -100m;
        var currencyCode = new CurrencyCode("PLN");
        var expectedMessage = "Amount cannot be negative";

        var ex = Assert.Throws<ArgumentException>(() => new Money(amount, currencyCode));
        Assert.Equal(expectedMessage, ex.Message);
    }
    
    [Fact]
    public void ItCastsMoneyToString()
    {
        var amount = 100m;
        var currencyCode = new CurrencyCode("PLN");
        var money = new Money(amount, currencyCode);
        
        Assert.Equal("PLN 100.00", money.ToString());
    }
}
