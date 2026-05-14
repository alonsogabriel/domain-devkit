namespace DomainDevKit;

public interface IEntity<TId, TValue>
    where TId : struct, IObjectId<TValue>
    where TValue : notnull
{
    TId Id { get; }
}

public abstract class Entity<TId, TValue> : IEntity<TId, TValue>
    where TId : struct, IObjectId<TValue>
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

        if (obj.GetType() != this.GetType())
            return false;

        if (other.IsTransient || this.IsTransient)
            return ReferenceEquals(this, other);

        return EqualityComparer<TId>.Default.Equals(other.Id, this.Id);
    }

    public override int GetHashCode()
    {
        if (this.IsTransient)
            return base.GetHashCode();

        return HashCode.Combine(this, Id);
    }

    public bool IsTransient
    {
        get
        {
            return EqualityComparer<TId>.Default.Equals(Id, default);
        }
    }
}