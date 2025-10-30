using CodesCampaigns.Application.Abstractions;
using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Jobs;
using Hangfire;

namespace CodesCampaigns.Application.Handlers;

public class GenerateTopUpCodesCommandHandler(IBackgroundJobClient backgroundJobs) : ICommandHandler<GenerateTopUpCodesCommand>
{
    public Task Handle(GenerateTopUpCodesCommand command, CancellationToken cancellationToken)
    {
        const int batchSize = 10;

        for (var i = 0; i < command.Count; i += batchSize)
        {
            var currentBatchSize = Math.Min(batchSize, command.Count - i);

            backgroundJobs.Enqueue<GenerateTopUpBatchJob>(job =>
                job.GenerateBatch(command, currentBatchSize, cancellationToken));
        }

        return Task.CompletedTask;
    }
}
