namespace DomainDevKit;

public interface IEntity<TId, TValue>
    where TId : ObjectId<TValue>
    where TValue : notnull
{
    TId Id { get; }
}

public abstract class Entity<TId, TValue> : IEntity<TId, TValue>
    where TId : ObjectId<TValue>
    where TValue : notnull
{
    protected Entity() { }
    protected Entity(TId id)
    {
        Id = id;
    }
    public TId Id { get; protected set; }
}