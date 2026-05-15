using System.Linq.Expressions;

namespace DomainDevKit;

public interface IRepository<T> where T : class
{
    Task<T?> FindAsync(object id, CancellationToken ct = default);
    Task<IEnumerable<T>> FindWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}