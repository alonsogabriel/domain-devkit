using System.Text.RegularExpressions;

namespace DomainDevKit;

public class StringValueOptions
{
    public bool NotEmpty { get; init; }
    public uint MinLength { get; init; }
    public uint? MaxLength { get; init; }
    public bool Trim { get; init; }
    public bool RemoveDuplicatedSpaces { get; init; }
    public Regex? Regex { get; init; }
}