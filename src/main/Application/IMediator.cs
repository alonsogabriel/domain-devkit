namespace DomainDevKit.Application;

public interface IMediator
{
    Task<TResponse> Handle<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
}

public class Mediator(IServiceProvider sp) : IMediator
{
    public Task<TResponse> Handle<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));

        var handler = sp.GetService(handlerType)
            ?? throw new InvalidOperationException($"Handler not registered for type '{handlerType.Name}';");

        var method = handlerType.GetMethod(nameof(ICommandHandler<,>.Handle))
            ?? throw new InvalidOperationException($"Failed to find handler method.");

        var response = method.Invoke(handler, [command, ct]);

        return (Task<TResponse>)response!;
    }
}