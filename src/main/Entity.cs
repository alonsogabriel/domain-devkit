namespace DomainDevKit;

public abstract class Entity<T> : IEntity<T>
    where T : notnull
{
    protected Entity() { }
    protected Entity(T id)
    {
        Id = id;
    }
    public T Id { get; protected set; }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj is not Entity<T> other)
            return false;

        if (obj.GetType() != this.GetType())
            return false;

        if (other.IsTransient || this.IsTransient)
            return ReferenceEquals(this, other);

        return EqualityComparer<T>.Default.Equals(other.Id, this.Id);
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
            return EqualityComparer<T>.Default.Equals(Id, default);
        }
    }
}