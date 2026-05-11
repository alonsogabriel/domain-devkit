namespace DomainDevKit.Drawing;

public readonly record struct Color(ColorComponent Red, ColorComponent Green, ColorComponent Blue, ColorComponent Alpha) : IValueObject
{
    public static Color FromRGBA(short red, short green, short blue, short alpha = ColorComponent.MAX_VALUE)
    {
        return new(new ColorComponent(red), new ColorComponent(green), new ColorComponent(blue), new ColorComponent(alpha));
    }

    public static Color FromHEX(string hexValue)
    {
        ArgumentException.ThrowIfNullOrEmpty(hexValue);

        uint value = Convert.ToUInt32(NormalizeHexValue(hexValue), 16);
        var comp = new short[4];

        for (int i = 0; i < comp.Length; i++)
        {
            comp[i] = (short)(value >> (8 * i) & ColorComponent.MAX_VALUE);
        }

        return FromRGBA(red: comp[3], green: comp[2], blue: comp[1], alpha: comp[0]);
    }

    public static string NormalizeHexValue(string value)
    {
        int length = value.Length;

        if (!length.IsIn(3, 4, 6, 8))
            throw new ArgumentException("Invalid length.");

        var normalized = new char[8];
        bool isShort = length == 3 || length == 4;

        for (int i = 0; i < normalized.Length; i++)
        {
            int j = (isShort ? i / 2 : i) % length;
            char ch = char.ToUpper(value[j]);

            if (!ch.IsInRange('0', '9') && !ch.IsInRange('A', 'F'))
                throw new InvalidOperationException($"Invalid character '{ch}' at index '{j}'.");

            normalized[i] = ch;
        }

        if (length == 3 || length == 6)
        {
            normalized[^1] = normalized[^2] = 'F';
        }

        return new string(normalized);
    }

    public static Color Random
    {
        get
        {
            var comp = new ColorComponent[3];

            for (int i = 0; i < comp.Length; i++)
                comp[i] = ColorComponent.Random;

            return new(comp[0], comp[1], comp[2], new(ColorComponent.MAX_VALUE));
        }
    }

    public Color CopyWith(short? red = null, short? green = null, short? blue = null, short? alpha = null)
    {
        return FromRGBA(
            red ?? Red.Value,
            green ?? Green.Value,
            blue ?? Blue.Value,
            alpha ?? Alpha.Value);
    }

    public override string ToString()
    {
        short[] comp = [Red.Value, Green.Value, Blue.Value, Alpha.Value];
        uint value = 0;

        for (int i = 0; i < comp.Length; i++)
        {
            int ix = comp.Length - i - 1;
            value |= (uint)(comp[ix] << (8 * i));
        }

        return value.ToString("X8");
    }
}