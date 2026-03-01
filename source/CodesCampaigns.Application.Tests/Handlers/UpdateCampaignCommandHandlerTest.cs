using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Exceptions;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class UpdateCampaignCommandHandlerTest
{
    private static readonly DateTime CreateTime = new(2024, 1, 1, 12, 0, 0);
    private static readonly DateTime UpdateTime = new(2024, 6, 1, 9, 0, 0);

    private readonly ICampaignsRepository _repository;
    private readonly IClock _createClock;
    private readonly UpdateCampaignCommandHandler _handler;

    public UpdateCampaignCommandHandlerTest()
    {
        _repository = Substitute.For<ICampaignsRepository>();
        _createClock = Substitute.For<IClock>();
        _createClock.Current.Returns(CreateTime);
        var updateClock = Substitute.For<IClock>();
        updateClock.Current.Returns(UpdateTime);
        _handler = new UpdateCampaignCommandHandler(_repository, updateClock);
    }

    [Fact]
    public async Task ItUpdatesCampaignName()
    {
        var campaign = Campaign.Create(CampaignId.Create(), "Old Name", _createClock);
        _repository.GetById(campaign.Id, Arg.Any<CancellationToken>()).Returns(campaign);

        await _handler.Handle(new UpdateCampaignCommand(campaign.Id, "New Name"), CancellationToken.None);

        Assert.Equal("New Name", campaign.Name);
        Assert.Equal(UpdateTime, campaign.UpdatedAt);
        await _repository.Received(1).Update(campaign, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ItThrowsCampaignNotFoundExceptionWhenCampaignDoesNotExist()
    {
        var missingId = CampaignId.Create();
        _repository.GetById(missingId, Arg.Any<CancellationToken>()).Returns((Campaign?)null);

        var ex = await Assert.ThrowsAsync<CampaignNotFoundException>(
            () => _handler.Handle(new UpdateCampaignCommand(missingId, "Name"), CancellationToken.None));

        Assert.Equal($"Campaign with ID: {(Guid)missingId} was not found.", ex.Message);
    }
}
