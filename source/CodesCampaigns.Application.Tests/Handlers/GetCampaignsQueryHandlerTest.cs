using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class GetCampaignsQueryHandlerTest
{
    private readonly ICampaignsRepository _repository;
    private readonly GetCampaignsQueryHandler _handler;
    private readonly IClock _clock;

    public GetCampaignsQueryHandlerTest()
    {
        _repository = Substitute.For<ICampaignsRepository>();
        _clock = Substitute.For<IClock>();
        _clock.Current.Returns(new DateTime(2024, 1, 1, 12, 0, 0));
        _handler = new GetCampaignsQueryHandler(_repository);
    }

    [Fact]
    public async Task ItReturnsAllCampaigns()
    {
        var campaigns = new List<Campaign>
        {
            Campaign.Create(CampaignId.Create(), "Campaign A", _clock),
            Campaign.Create(CampaignId.Create(), "Campaign B", _clock),
        };
        _repository.GetAll(Arg.Any<CancellationToken>()).Returns(campaigns);

        var result = (await _handler.Handle(new GetCampaignsQuery(), CancellationToken.None)).ToList();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ItReturnsEmptyListWhenNoCampaignsExist()
    {
        _repository.GetAll(Arg.Any<CancellationToken>()).Returns([]);

        var result = await _handler.Handle(new GetCampaignsQuery(), CancellationToken.None);

        Assert.Empty(result);
    }
}
