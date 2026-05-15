using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DomainDevKit.EFCore;

public class UnitOfWork<T>(T context) : IUnitOfWork where T : DbContext
{
    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        var transaction = await context.Database.BeginTransactionAsync(ct); 

        return new UnitOfWorkTransaction(transaction);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}

public class UnitOfWorkTransaction(IDbContextTransaction transaction) : IUnitOfWorkTransaction
{
    public async Task CommitAsync(CancellationToken ct = default)
    {
        await transaction.CommitAsync(ct);
    }

    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        await transaction.RollbackAsync(ct);
    }
}