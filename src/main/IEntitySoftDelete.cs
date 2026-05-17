namespace DomainDevKit;

public abstract class EntitySoftDelete<T>
    : EntityTimestamps<T>, ISoftDelete where T : notnull
{

    protected EntitySoftDelete() { }
    protected EntitySoftDelete(T id) : base(id) { }
    public DateTime? DeletedAt { get; protected set; }

    public void Delete()
    {
        Delete(DateTime.UtcNow);
    }

    public virtual void Delete(DateTime now)
    {
        if (DeletedAt.HasValue)
            return;

        DeletedAt = now;
    }

    public virtual void Restore()
    {
        if (!DeletedAt.HasValue)
            return;

        DeletedAt = null;
    }
}