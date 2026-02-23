using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class GetCampaignCodesQueryHandlerTest
{
    private static readonly DateTime FixedTime = new(2024, 1, 1, 12, 0, 0);

    private readonly ITopUpsRepository _repository;
    private readonly GetCampaignCodesQueryHandler _handler;

    public GetCampaignCodesQueryHandlerTest()
    {
        _repository = Substitute.For<ITopUpsRepository>();
        _handler = new GetCampaignCodesQueryHandler(_repository);
    }

    [Fact]
    public async Task ItReturnsTopUpCodesForGivenCampaign()
    {
        var campaignId = CampaignId.Create();
        var topUps = new List<TopUp> { MakeTopUp(campaignId), MakeTopUp(campaignId) };
        _repository.GetByCampaignId(campaignId, Arg.Any<CancellationToken>()).Returns(topUps);

        var result = (await _handler.Handle(new GetCampaignCodesQuery(campaignId), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(campaignId, t.CampaignId));
    }

    [Fact]
    public async Task ItReturnsEmptyListWhenNoCodesExistForCampaign()
    {
        var campaignId = CampaignId.Create();
        _repository.GetByCampaignId(campaignId, Arg.Any<CancellationToken>()).Returns([]);

        var result = (await _handler.Handle(new GetCampaignCodesQuery(campaignId), CancellationToken.None)).ToList();

        Assert.Empty(result);
    }

    private static TopUp MakeTopUp(CampaignId campaignId) =>
        TopUp.Create(TopUpCode.Create(), new Money(10m, new CurrencyCode("PLN")), campaignId, FixedTime);
}
