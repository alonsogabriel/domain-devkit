namespace DomainDevKit;

public abstract class ObjectId<T> : IValueObject<T> where T : notnull
{
    protected ObjectId() { }
    protected ObjectId(T value)
    {
        Value = value;
    }
    public T Value { get; protected set; }
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj is not ObjectId<T> other)
            return false;

        if (other.GetType() != this.GetType())
            return false;

        if (other.IsTransient || this.IsTransient)
            return ReferenceEquals(this, other);

        return EqualityComparer<T>.Default.Equals(other.Value, this.Value);
    }

    public override int GetHashCode()
    {
        if (this.IsTransient)
            return base.GetHashCode();

        return this.Value.GetHashCode();
    }

    public bool IsTransient
    {
        get
        {
            return EqualityComparer<T>.Default.Equals(this.Value, default(T));
        }
    }
}