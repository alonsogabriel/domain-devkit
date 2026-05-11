using Microsoft.Extensions.DependencyInjection;

namespace DomainDevKit.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandHandler<T>(this IServiceCollection services)
        where T : class, ICommandHandler
    {
        return services.AddCommandHandler(typeof(T));
    }
    public static IServiceCollection AddCommandHandler(this IServiceCollection services, Type handlerType)
    {
        if (!handlerType.IsClass)
            throw new ArgumentException("Value must be a reference type.", nameof(handlerType));

        if (!typeof(ICommandHandler).IsAssignableFrom(handlerType))
            throw new ArgumentException($"Type does not implement '{nameof(ICommandHandler)}'.");

        var genericHandler = typeof(ICommandHandler<,>);

        var interfaces = handlerType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition().Equals(genericHandler));

        foreach (var i in interfaces)
        {
            services.AddScoped(i, handlerType);
        }

        return services;
    }

    public static IServiceCollection AddMediator<T>(this IServiceCollection services)
        where T : class, IMediator
    {
        return services.AddScoped<IMediator, T>();
    }

    public static IServiceCollection AddMediator(this IServiceCollection services) => services.AddMediator<Mediator>();
}