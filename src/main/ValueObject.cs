namespace DomainDevKit;

public abstract record ValueObject<T> : IValueObject<T> where T : notnull
{
    protected ValueObject() { }
    public ValueObject(T value)
    {
        Validate(value);
        Value = value;
    }
    public T Value { get; protected set; }
    protected virtual void Validate(T value) { }
}
