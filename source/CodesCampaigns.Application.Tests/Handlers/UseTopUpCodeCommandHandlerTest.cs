using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class UseTopUpCodeCommandHandlerTest
{
    private static readonly DateTime FixedTime = new(2024, 1, 1, 12, 0, 0);
    private static readonly Money ValidMoney = new(10m, new CurrencyCode("PLN"));

    private readonly ITopUpsRepository _repository;
    private readonly ICampaignsRepository _campaignsRepository;
    private readonly IClock _clock;
    private readonly UseTopUpCodeCommandHandler _handler;

    public UseTopUpCodeCommandHandlerTest()
    {
        _repository = Substitute.For<ITopUpsRepository>();
        _campaignsRepository = Substitute.For<ICampaignsRepository>();
        _clock = Substitute.For<IClock>();
        _clock.Current.Returns(FixedTime);
        _campaignsRepository.GetById(Arg.Any<CampaignId>(), Arg.Any<CancellationToken>())
            .Returns((Campaign?)null);
        _repository.CountUsedByEmailAndCampaignForMonth(
                Arg.Any<string>(), Arg.Any<CampaignId>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(0);
        _handler = new UseTopUpCodeCommandHandler(_repository, _campaignsRepository, _clock);
    }

    [Fact]
    public async Task ItReturnsFailureWhenCodeNotFound()
    {
        var code = Guid.NewGuid();
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns((TopUp?)null);

        var result = await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("invalid_data", result.ErrorCode);
    }

    [Fact]
    public async Task ItReturnsFailureWhenCodeIsAlreadyUsed()
    {
        var code = Guid.NewGuid();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, CampaignId.Create(), FixedTime);
        topUp.Use("previous@example.com", "OLD_PARTNER", FixedTime);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);

        var result = await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("invalid_data", result.ErrorCode);
    }

    [Fact]
    public async Task ItDoesNotUpdateRepositoryWhenCodeNotFound()
    {
        var code = Guid.NewGuid();
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns((TopUp?)null);

        await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        await _repository.DidNotReceive().Update(Arg.Any<TopUp>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ItDoesNotUpdateRepositoryWhenCodeIsAlreadyUsed()
    {
        var code = Guid.NewGuid();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, CampaignId.Create(), FixedTime);
        topUp.Use("previous@example.com", "OLD_PARTNER", FixedTime);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);

        await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        await _repository.DidNotReceive().Update(Arg.Any<TopUp>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ItReturnsSuccessWhenCodeIsValid()
    {
        var code = Guid.NewGuid();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, CampaignId.Create(), FixedTime);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);

        var result = await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        Assert.True(result.Success);
        Assert.Null(result.ErrorCode);
    }

    [Fact]
    public async Task ItUpdatesTopUpWithCorrectFieldsWhenCodeIsValid()
    {
        var code = Guid.NewGuid();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, CampaignId.Create(), FixedTime);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);

        await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        Assert.Equal("user@example.com", topUp.Email);
        Assert.Equal("PARTNER", topUp.PartnerCode);
        Assert.Equal(FixedTime, topUp.UsedAt);
    }

    [Fact]
    public async Task ItCallsRepositoryUpdateWhenCodeIsValid()
    {
        var code = Guid.NewGuid();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, CampaignId.Create(), FixedTime);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);

        await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        await _repository.Received(1).Update(topUp, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ItReturnsUsageLimitExceededWhenMonthlyLimitReached()
    {
        var code = Guid.NewGuid();
        var campaignId = CampaignId.Create();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, campaignId, FixedTime);
        var campaign = Campaign.Create(campaignId, "Test", _clock, maxNumberOfTopUpsPerUser: 3);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);
        _campaignsRepository.GetById(campaignId, Arg.Any<CancellationToken>()).Returns(campaign);
        _repository.CountUsedByEmailAndCampaignForMonth(
                Arg.Any<string>(), Arg.Any<CampaignId>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(3);

        var result = await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("usage_limit_exceeded", result.ErrorCode);
    }

    [Fact]
    public async Task ItDoesNotUpdateRepositoryWhenMonthlyLimitReached()
    {
        var code = Guid.NewGuid();
        var campaignId = CampaignId.Create();
        var topUp = TopUp.Create(new TopUpCode(code), ValidMoney, campaignId, FixedTime);
        var campaign = Campaign.Create(campaignId, "Test", _clock, maxNumberOfTopUpsPerUser: 3);
        _repository.GetByCode(Arg.Any<TopUpCode>(), Arg.Any<CancellationToken>()).Returns(topUp);
        _campaignsRepository.GetById(campaignId, Arg.Any<CancellationToken>()).Returns(campaign);
        _repository.CountUsedByEmailAndCampaignForMonth(
                Arg.Any<string>(), Arg.Any<CampaignId>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(3);

        await _handler.Handle(new UseTopUpCodeCommand("PARTNER", code, "user@example.com"), CancellationToken.None);

        await _repository.DidNotReceive().Update(Arg.Any<TopUp>(), Arg.Any<CancellationToken>());
    }
}
