namespace DomainDevKit;

public interface IValueObject;

public interface IValueObject<T> : IValueObject where T : notnull
{
    T Value { get; }
}