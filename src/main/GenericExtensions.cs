using System.Diagnostics.CodeAnalysis;

namespace DomainDevKit;

public static class GenericExtensions
{
    public static bool IsNull<T>([NotNullWhen(false)] this T? value) => value is null;
    public static bool IsNotNull<T>([NotNullWhen(true)] this T? value) => value is not null;
    public static T ForceIntoRange<T>(this T value, T min, T max) where T : IComparable<T>
    {
        if (min.IsGreaterThan(max))
        {
            (min, max) = (max, min);
        }

        if (value.IsLessThan(min))
            return min;

        if (value.IsGreaterThan(max))
            return max;

        return value;
    }

    public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) > 0;
    }

    public static bool IsGreaterThanOrEqual<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) >= 0;
    }

    public static bool IsLessThan<T>(this T value, T other) where T : IComparable<T>
    {
        return value.CompareTo(other) < 0;
    }

    public static bool IsLessThanOrEqual<T>(this T value, T other) where T: IComparable<T>
    {
        return value.CompareTo(other) <= 0;
    }

    public static bool IsIn<T>(this T value, params T[] items) => items.Contains(value);

    public static bool IsInRange<T>(this T value, T min, T max) where T : IComparable<T>
    {
        if (min.IsGreaterThan(max))
        {
            (min, max) = (max, min);
        }

        return value.IsGreaterThanOrEqual(min) && value.IsLessThanOrEqual(max); 
    }
}