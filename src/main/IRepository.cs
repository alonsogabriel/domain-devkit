using System.Linq.Expressions;

namespace DomainDevKit;

public interface IRepository<T> where T : class
{
    Task<T?> Find(object id, CancellationToken ct = default);
    Task<IEnumerable<T>> FindWhere(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task Save(T entity, CancellationToken ct = default);
    Task Delete(T entity, CancellationToken ct = default);
}