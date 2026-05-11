namespace DomainDevKit.Application;

public interface ICommandHandler;

public interface ICommandHandler<TCommand, TResponse> : ICommandHandler
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken ct = default);
}