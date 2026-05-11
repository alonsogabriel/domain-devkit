using DomainDevKit.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Application;

public class MediatorTests(ITestOutputHelper output)
{
    [Fact]
    public async Task AddCommandHandlerShouldSucceed()
    {
        // Given
        var builder = Host.CreateDefaultBuilder();
        builder.ConfigureServices((ctx, services) =>
        {
            services.AddMediator()
                .AddCommandHandler<OrderHandler>();
        });
        using var app = builder.Build();
        using var scope = app.Services.CreateScope();

        // When
        var handler = scope.ServiceProvider.GetRequiredService<IMediator>();
        var result = await handler.Handle(new CreateOrder("Gabriel"));

        // Then
        output.WriteLine(result.ToString());
        Assert.NotNull(result);
    }
}

internal record CreateOrder(string Customer) : ICommand<OrderCreated>;

internal record OrderCreated(Guid Id);

internal class OrderHandler :
    ICommandHandler<CreateOrder, OrderCreated>
{
    public Task<OrderCreated> Handle(CreateOrder command, CancellationToken ct = default)
    {
        return Task.FromResult(new OrderCreated(Guid.NewGuid()));
    }
}