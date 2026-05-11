using DomainDevKit.Identity;

namespace DomainDevKit;

public static class DomainExtensions
{
    public static int? GetAge(this IPersonBase person, DateTime now)
    {
        return person.BirthDate?.GetAge(now);
    }
    public static int? GetAge(this IPersonBase person)
    {
        return person.BirthDate?.GetAge();
    }

    public static bool IsSoftDeleted(this ISoftDelete value)
    {
        return value.DeletedAt.HasValue;
    }
}