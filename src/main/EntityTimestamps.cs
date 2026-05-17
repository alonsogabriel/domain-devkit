namespace DomainDevKit;

public abstract class EntityTimestamps<T>
    : Entity<T>, IEntityTimestamps<T> where T : notnull
{
    protected EntityTimestamps() { }
    protected EntityTimestamps(T id) : base(id)
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