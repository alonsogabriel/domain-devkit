using System.Linq.Expressions;
using DomainDevKit;
using Microsoft.EntityFrameworkCore;

namespace DomainDevKit.EFCore;

public abstract class EFRepositoryBase<T, C>(C context) : IRepository<T>
    where T : class
    where C : DbContext
{
    public async Task<T?> Find(object id, CancellationToken ct = default)
    {
        return await context.FindAsync<T>([id], ct);
    }

    public async Task<IEnumerable<T>> FindWhere(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await context.Set<T>().Where(predicate).ToListAsync(ct);
    }

    public async Task Save(T entity, CancellationToken ct = default)
    {
        var entry = context.Entry(entity);

        if (entry.State == EntityState.Unchanged)
            return;

        if (entry.State == EntityState.Detached)
        {
            await context.AddAsync(entity, ct);
        }

        await context.SaveChangesAsync(ct);
    }

    public async Task Delete(T entity, CancellationToken ct = default)
    {
        context.Remove(entity);

        await context.SaveChangesAsync(ct);
    }
}