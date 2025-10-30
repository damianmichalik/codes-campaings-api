﻿namespace CodesCampaigns.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}
