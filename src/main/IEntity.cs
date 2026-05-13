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
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj is not Entity<TId, TValue> other)
            return false;

        if (other.GetType() != this.GetType())
            return false;

        return this.Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}