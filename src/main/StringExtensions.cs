using System.Text;

namespace DomainDevKit;

public static class StringExtensions
{
    public static string RemoveExtraSpaces(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var sb = new StringBuilder(value.Length);
        char p = ' ';

        foreach(var c in value)
        {
            if (c == ' ' && c == p)
                continue;

            sb.Append(c);
            p = c;
        }

        if (p == ' ')
        {
            sb.Length--;
        }

        return sb.ToString();
    }
}
