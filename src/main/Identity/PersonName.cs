namespace DomainDevKit.Identity;

public sealed record PersonName : IValueObject
{
    private PersonName() { }
    public PersonName(string firstName, string? middleName, string lastName, string? suffix)
    {
        FirstName = Handle(firstName);

        if (middleName.IsNotNull())
        {
            MiddleName = Handle(middleName);
        }

        LastName = Handle(lastName);
        
        if (suffix.IsNotNull())
        {
            Suffix = Handle(suffix);
        }
    }
    public string FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string? Suffix { get; private set; }
    public string FullName
    {
        get
        {
            List<string> names = [];

            names.Add(FirstName);

            if (MiddleName.IsNotNull())
            {
                names.Add(MiddleName);
            }

            names.Add(LastName);

            if (Suffix.IsNotNull())
            {
                names.Add(Suffix);
            }

            return string.Join(' ', names);
        }
    }

    public override string ToString() => FullName;
    
    private static string Handle(string value)
    {
        value = value.RemoveExtraSpaces();

        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("Value cannot be empty.", nameof(value));

        if (value.Length > 50)
            throw new ArgumentException("Value is too long.");

        return value;
    }
}