namespace DomainDevKit.Identity;

public sealed record BirthDate : ValueObject<DateOnly>
{
    private BirthDate() { }
    public BirthDate(DateOnly value)
    {
        Value = value;
    }

    public int GetAge(DateTime dateFrom)
    {
        var today = DateOnly.FromDateTime(dateFrom);

        if (Value > today)
            return -1;

        int years = today.Year - Value.Year;
        int months = today.Month - Value.Month;
        int days = today.Day - Value.Day;

        if (months < 0 || months == 0 && days < 0)
            --years;

        return years;
    }

    public int GetAge() => GetAge(DateTime.UtcNow);
}