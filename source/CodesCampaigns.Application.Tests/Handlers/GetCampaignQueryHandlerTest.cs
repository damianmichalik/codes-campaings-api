using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Queries;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class GetCampaignQueryHandlerTest
{
    private readonly ICampaignsRepository _repository;
    private readonly GetCampaignQueryHandler _handler;
    private readonly IClock _clock;

    public GetCampaignQueryHandlerTest()
    {
        _repository = Substitute.For<ICampaignsRepository>();
        _clock = Substitute.For<IClock>();
        _clock.Current.Returns(new DateTime(2024, 1, 1, 12, 0, 0));
        _handler = new GetCampaignQueryHandler(_repository);
    }

    [Fact]
    public async Task ItReturnsCampaignWhenFound()
    {
        var campaign = Campaign.Create(CampaignId.Create(), "Test Campaign", _clock);
        _repository.GetById(campaign.Id, Arg.Any<CancellationToken>()).Returns(campaign);

        var result = await _handler.Handle(new GetCampaignQuery(campaign.Id), CancellationToken.None);

        Assert.Equal(campaign.Id, result.Id);
        Assert.Equal(campaign.Name, result.Name);
    }

    [Fact]
    public async Task ItThrowsCampaignNotFoundExceptionWhenCampaignDoesNotExist()
    {
        var missingId = CampaignId.Create();
        _repository.GetById(missingId, Arg.Any<CancellationToken>()).Returns((Campaign?)null);

        var ex = await Assert.ThrowsAsync<CampaignNotFoundException>(
            () => _handler.Handle(new GetCampaignQuery(missingId), CancellationToken.None));

        Assert.Equal($"Campaign with ID: {(Guid)missingId} was not found.", ex.Message);
    }
}
