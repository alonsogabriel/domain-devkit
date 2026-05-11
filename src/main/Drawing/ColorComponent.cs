using System.Security.Cryptography;

namespace DomainDevKit.Drawing;

public readonly record struct ColorComponent : IValueObject<short>
{
    public const short MIN_VALUE = 0;
    public const short MAX_VALUE = 255;
    public ColorComponent() : this(MIN_VALUE) { }
    public ColorComponent(short value)
    {
        Value = value.ForceIntoRange(MIN_VALUE, MAX_VALUE);
    }

    public static ColorComponent Random => new((short)RandomNumberGenerator.GetInt32(MIN_VALUE, MAX_VALUE + 1));

    public short Value { get; private init; }
}