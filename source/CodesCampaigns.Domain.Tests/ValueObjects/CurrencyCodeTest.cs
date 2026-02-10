using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.ValueObjects;

public class CurrencyCodeTest
{
    [Fact]
    public void ItCreatesCurrencyCodeObject()
    {
        var code = "PLN";
        var currencyCode = new CurrencyCode(code);

        Assert.Equal(code, currencyCode.Code);
    }
    
    [Fact]
    public void ItCreatesCurrencyCodeObjectFromLowercaseLettersCode()
    {
        var code = "pln";
        var expectedCode = "PLN";
        var currencyCode = new CurrencyCode(code);

        Assert.Equal(expectedCode, currencyCode.Code);
    }
    
    [Fact]
    public void ItCastsCurrencyCodeToString()
    {
        var code = "PLN";
        var currencyCode = new CurrencyCode(code);
        
        Assert.Equal("PLN", currencyCode.ToString());
    }
    
    [Fact]
    public void ItThrowsExceptionWhenCodeIsEmpty()
    {
        var code = "";
        var expectedMessage = "Currency code cannot be empty";

        var ex = Assert.Throws<ArgumentException>(() => new CurrencyCode(code));
        Assert.Equal(expectedMessage, ex.Message);
    }
    
    [Theory]
    [InlineData("PL")]
    [InlineData("PLNN")]
    public void ItThrowsExceptionWhenCodeLengthIsNotEqualToThree(string code)
    {
        var expectedMessage = "Currency code must be exactly 3 letters (ISO 4217)";

        var ex = Assert.Throws<ArgumentException>(() => new CurrencyCode(code));
        Assert.Equal(expectedMessage, ex.Message);
    }
}
