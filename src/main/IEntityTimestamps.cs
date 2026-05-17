namespace DomainDevKit;

public interface IEntityTimestamps<T> : IEntity<T>
    where T : notnull
{
    DateTime CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}