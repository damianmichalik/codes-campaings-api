using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Jobs;
using CodesCampaigns.Domain.Repositories;
using NSubstitute;

namespace CodesCampaigns.Application.Tests.Jobs;

public class GenerateTopUpBatchJobTest
{
    private readonly ITopUpsRepository _repository;
    private readonly GenerateTopUpBatchJob _job;

    public GenerateTopUpBatchJobTest()
    {
        _repository = Substitute.For<ITopUpsRepository>();
        _job = new GenerateTopUpBatchJob(_repository);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task ItAddsExactCountOfTopUps(int count)
    {
        var command = MakeCommand();

        await _job.GenerateBatch(command, count, CancellationToken.None);

        await _repository.Received(1).AddMany(
            Arg.Is<IReadOnlyCollection<Domain.Entities.TopUp>>(t => t.Count == count),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task ItCreatesTopUpsWithValuesFromCommand()
    {
        var command = MakeCommand();
        IReadOnlyCollection<Domain.Entities.TopUp>? captured = null;
        await _repository.AddMany(
            Arg.Do<IReadOnlyCollection<Domain.Entities.TopUp>>(t => captured = t),
            Arg.Any<CancellationToken>()
        );

        await _job.GenerateBatch(command, 3, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.All(captured, t =>
        {
            Assert.Equal((Guid)command.CampaignId, (Guid)t.CampaignId);
            Assert.Equal(command.Value, t.Value.Amount);
            Assert.Equal(command.Currency.ToUpperInvariant(), t.Value.CurrencyCode.Code);
            Assert.Equal(command.DateTime, t.CreatedAt);
        });
    }

    [Fact]
    public async Task ItGeneratesUniqueCodeForEachTopUp()
    {
        IReadOnlyCollection<Domain.Entities.TopUp>? captured = null;
        await _repository.AddMany(
            Arg.Do<IReadOnlyCollection<Domain.Entities.TopUp>>(t => captured = t),
            Arg.Any<CancellationToken>()
        );

        await _job.GenerateBatch(MakeCommand(), 5, CancellationToken.None);

        Assert.NotNull(captured);
        var codes = captured.Select(t => (Guid)t.Code).ToList();
        Assert.Equal(codes.Count, codes.Distinct().Count());
    }

    [Fact]
    public async Task ItCallsAddManyExactlyOnce()
    {
        await _job.GenerateBatch(MakeCommand(), 5, CancellationToken.None);

        await _repository.Received(1).AddMany(
            Arg.Any<IReadOnlyCollection<Domain.Entities.TopUp>>(),
            Arg.Any<CancellationToken>()
        );
    }

    private static GenerateTopUpCodesCommand MakeCommand() =>
        new(Guid.NewGuid(), 5, 25.50m, "PLN", new DateTime(2024, 1, 1, 12, 0, 0));
}
