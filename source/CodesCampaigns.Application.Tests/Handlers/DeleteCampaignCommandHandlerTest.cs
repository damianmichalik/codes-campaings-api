using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class DeleteCampaignCommandHandlerTest
{
    private readonly ICampaignsRepository _repository;
    private readonly DeleteCampaignCommandHandler _handler;
    private readonly IClock _clock;

    public DeleteCampaignCommandHandlerTest()
    {
        _repository = Substitute.For<ICampaignsRepository>();
        _clock = Substitute.For<IClock>();
        _clock.Current.Returns(new DateTime(2024, 1, 1, 12, 0, 0));
        _handler = new DeleteCampaignCommandHandler(_repository);
    }

    [Fact]
    public async Task ItDeletesCampaignWhenFound()
    {
        var campaign = Campaign.Create(CampaignId.Create(), "Test Campaign", _clock);
        _repository.GetById(campaign.Id, Arg.Any<CancellationToken>()).Returns(campaign);

        await _handler.Handle(new DeleteCampaignCommand(campaign.Id), CancellationToken.None);

        await _repository.Received(1).Delete(campaign, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ItThrowsCampaignNotFoundExceptionWhenCampaignDoesNotExist()
    {
        var missingId = CampaignId.Create();
        _repository.GetById(missingId, Arg.Any<CancellationToken>()).Returns((Campaign?)null);

        var ex = await Assert.ThrowsAsync<CampaignNotFoundException>(
            () => _handler.Handle(new DeleteCampaignCommand(missingId), CancellationToken.None));

        Assert.Equal($"Campaign with ID: {(Guid)missingId} was not found.", ex.Message);
    }
}
