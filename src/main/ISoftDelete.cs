namespace DomainDevKit;

public interface ISoftDelete
{
    DateTime? DeletedAt { get; }
    void Delete();
    void Restore();
}