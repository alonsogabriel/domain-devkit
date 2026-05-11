namespace DomainDevKit;

public abstract record StringValue : ValueObject<string>
{
    protected StringValue() { }
    public StringValue(string value) : base(value)
    {
    }

    public StringValue(string value, StringValueOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.RemoveDuplicatedSpaces)
        {
            value = value.RemoveExtraSpaces();
        }

        if (options.Trim && !options.RemoveDuplicatedSpaces)
        {
            value = value?.Trim() ?? string.Empty;
        }

        const string paramName = nameof(value);

        if (options.NotEmpty && string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);

        if (value.Length < options.MinLength)
            throw new ArgumentException($"Value is too short. Length is {value.Length} while min length is {options.MinLength}.", paramName);

        if (options.MaxLength.HasValue && value.Length > options.MaxLength.Value)
            throw new ArgumentException($"Value is too long. Length is {value.Length} while max length is {options.MaxLength}.");

        if (options.Regex.IsNotNull() && !options.Regex.IsMatch(value))
            throw new ArgumentException($"Value does not match pattern.");

        Validate(value);
        Value = value;
    }

    public int Length => Value.Length;

    public override string ToString() => Value;
}
