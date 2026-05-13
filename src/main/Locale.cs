namespace DomainDevKit;

public sealed record Locale : StringValue
{
    private Locale() { }
    public Locale(string value) : base(value) { }
}