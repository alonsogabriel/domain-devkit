namespace DomainDevKit;

public interface IUnitOfWork
{
    Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IUnitOfWorkTransaction : IAsyncDisposable
{
    Task RollbackAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
}