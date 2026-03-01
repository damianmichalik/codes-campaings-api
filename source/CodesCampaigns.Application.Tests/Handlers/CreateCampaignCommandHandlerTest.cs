using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Domain.Abstractions;
using CodesCampaigns.Domain.Entities;
using CodesCampaigns.Domain.Repositories;
using CodesCampaigns.Domain.ValueObjects;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class CreateCampaignCommandHandlerTest
{
    private static readonly DateTime FixedTime = new(2024, 1, 1, 12, 0, 0);

    private readonly ICampaignsRepository _repository;
    private readonly CreateCampaignCommandHandler _handler;

    public CreateCampaignCommandHandlerTest()
    {
        _repository = Substitute.For<ICampaignsRepository>();
        var clock = Substitute.For<IClock>();
        clock.Current.Returns(FixedTime);
        _handler = new CreateCampaignCommandHandler(_repository, clock);
    }

    [Fact]
    public async Task ItAddsCampaignToRepository()
    {
        var campaignId = CampaignId.Create();
        var command = new CreateCampaignCommand(campaignId, "Summer Campaign");

        await _handler.Handle(command, CancellationToken.None);

        await _repository.Received(1).Add(
            Arg.Is<Campaign>(c => c.Id == campaignId && c.Name == "Summer Campaign" && c.CreatedAt == FixedTime),
            Arg.Any<CancellationToken>()
        );
    }
}
