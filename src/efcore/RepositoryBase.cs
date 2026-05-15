using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DomainDevKit.EFCore;

public abstract class RepositoryBase<T, C>(C context) : IRepository<T>
    where T : class
    where C : DbContext
{
    public async Task<T?> FindAsync(object id, CancellationToken ct = default)
    {
        return await context.FindAsync<T>([id], ct);
    }

    public async Task<IEnumerable<T>> FindWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await context.Set<T>().Where(predicate).ToListAsync(ct);
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await context.AddAsync(entity, ct);
    }
    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        context.Update(entity);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        context.Remove(entity);

        return Task.CompletedTask;
    }
}