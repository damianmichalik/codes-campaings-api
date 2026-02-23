using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.ValueObjects;

namespace CodesCampaigns.Domain.Tests.ValueObjects;

public class TopUpCodeTest
{
    [Fact]
    public void ItCreatesTopUpCodeFromValidGuid()
    {
        var guid = Guid.NewGuid();
        var topUpCode = new TopUpCode(guid);

        Assert.Equal(guid, topUpCode.Value);
    }

    [Fact]
    public void ItThrowsExceptionWhenGuidIsEmpty()
    {
        var ex = Assert.Throws<InvalidEntityIdException>(() => new TopUpCode(Guid.Empty));

        Assert.Equal($"Cannot set: {Guid.Empty} as entity identifier.", ex.Message);
    }

    [Fact]
    public void ItCreatesUniqueTopUpCodeViaFactoryMethod()
    {
        var first = TopUpCode.Create();
        var second = TopUpCode.Create();

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void ItConvertsImplicitlyToGuid()
    {
        var guid = Guid.NewGuid();
        var topUpCode = new TopUpCode(guid);

        Guid result = topUpCode;

        Assert.Equal(guid, result);
    }

    [Fact]
    public void ItConvertsImplicitlyFromGuid()
    {
        var guid = Guid.NewGuid();

        TopUpCode topUpCode = guid;

        Assert.Equal(guid, topUpCode.Value);
    }

    [Fact]
    public void ItReturnsGuidStringFromToString()
    {
        var guid = Guid.NewGuid();
        var topUpCode = new TopUpCode(guid);

        Assert.Equal(guid.ToString(), topUpCode.ToString());
    }

    [Fact]
    public void ItIsEqualToAnotherTopUpCodeWithSameGuid()
    {
        var guid = Guid.NewGuid();
        var first = new TopUpCode(guid);
        var second = new TopUpCode(guid);

        Assert.Equal(first, second);
    }
}