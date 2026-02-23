using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Handlers;
using CodesCampaigns.Application.Jobs;
using Hangfire;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Handlers;

public class GenerateTopUpCodesCommandHandlerTest
{
    private readonly IBackgroundJobClient _client;
    private readonly GenerateTopUpCodesCommandHandler _handler;

    public GenerateTopUpCodesCommandHandlerTest()
    {
        _client = Substitute.For<IBackgroundJobClient>();
        _handler = new GenerateTopUpCodesCommandHandler(_client);
    }

    [Fact]
    public async Task ItEnqueuesSingleJobWhenCountIsLessThanBatchSize()
    {
        await _handler.Handle(MakeCommand(5), CancellationToken.None);

        _client.Received(1).Create(
            Arg.Is<Hangfire.Common.Job>(j => j.Type == typeof(GenerateTopUpBatchJob)),
            Arg.Any<Hangfire.States.IState>()
        );
    }

    [Fact]
    public async Task ItEnqueuesSingleJobWhenCountEqualsExactlyOneBatch()
    {
        await _handler.Handle(MakeCommand(10), CancellationToken.None);

        _client.Received(1).Create(
            Arg.Is<Hangfire.Common.Job>(j => j.Type == typeof(GenerateTopUpBatchJob)),
            Arg.Any<Hangfire.States.IState>()
        );
    }

    [Theory]
    [InlineData(11, 2)]
    [InlineData(20, 2)]
    [InlineData(21, 3)]
    [InlineData(25, 3)]
    public async Task ItEnqueuesCorrectNumberOfBatchJobsForLargerCounts(int count, int expectedJobs)
    {
        await _handler.Handle(MakeCommand(count), CancellationToken.None);

        _client.Received(expectedJobs).Create(
            Arg.Is<Hangfire.Common.Job>(j => j.Type == typeof(GenerateTopUpBatchJob)),
            Arg.Any<Hangfire.States.IState>()
        );
    }

    private static GenerateTopUpCodesCommand MakeCommand(int count) =>
        new(Guid.NewGuid(), count, 10m, "PLN", DateTime.UtcNow);
}
