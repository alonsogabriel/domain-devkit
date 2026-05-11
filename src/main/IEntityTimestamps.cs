namespace DomainDevKit;

public interface IEntityTimestamps<TId, TValue> : IEntity<TId, TValue>
    where TId : ObjectId<TValue>
    where TValue : notnull
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}

public abstract class EntityTimestamps<TId, TValue>
    : Entity<TId, TValue>, IEntityTimestamps<TId, TValue>
    where TId : ObjectId<TValue>
    where TValue : notnull
{
    protected EntityTimestamps() { }
    protected EntityTimestamps(TId id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    protected virtual void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}