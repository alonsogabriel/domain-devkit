namespace DomainDevKit;

public abstract class EntitySoftDelete<TId, TValue>
    : EntityTimestamps<TId, TValue>, ISoftDelete
    where TId : struct, IObjectId<TValue>
    where TValue : notnull
{

    protected EntitySoftDelete() { }
    protected EntitySoftDelete(TId id) : base(id) { }
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