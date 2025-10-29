using CodesCampaigns.Application.Commands;
using CodesCampaigns.Application.Jobs;
using CodesCampaigns.Application.ValueObjects;
using Hangfire;
using MediatR;

namespace CodesCampaigns.Application.Handlers;

public class GenerateTopUpCodesCommandHandler(IBackgroundJobClient backgroundJobs) : IRequestHandler<GenerateTopUpCodesCommand>
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
