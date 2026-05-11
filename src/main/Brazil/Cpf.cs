using System.ComponentModel;
using System.Security.Cryptography;

namespace DomainDevKit.Brazil;

public enum CpfVerifierDigit
{
    First = 1,
    Second = 2
}

public sealed record Cpf : StringValue
{
    private Cpf() { }
    public Cpf(string value)
    {
        Value = Handle(value);
    }

    public string Formatted
    {
        get
        {
            var format = new char[14];

            for (int i = 0; i < Value.Length; i++)
                format[i + i / 3] = Value[i];

            format[3] = format[7] = '.';
            format[^3] = '-';

            return new string(format);
        }
    }

    public override string ToString() => Formatted;

    private static string Handle(string value)
    {
        bool isFormatted = value.Length == 14;

        if (!isFormatted && value.Length != 11)
            throw new ArgumentException("Invalid length.");

        if (isFormatted && (value[3] != '.' || value[7] != '.' || value[^3] != '-'))
            throw new InvalidOperationException("Invalid format.");

        var cpf = new int[11];
        bool isSingleDigit = true;

        for (int i = 0; i < cpf.Length; i++)
        {
            int j = isFormatted ? i / 3 : 0;
            char d = value[i + j];

            if (!char.IsDigit(d))
                throw new InvalidOperationException($"Invalid digit '{value[d]}'.");

            cpf[i] = d - '0';

            if (i > 0)
            {
                isSingleDigit = isSingleDigit && cpf[i] == cpf[i - 1];
            }
        }

        int d1 = CalculateDigit(cpf, CpfVerifierDigit.First);
        int d2 = CalculateDigit(cpf, CpfVerifierDigit.Second);
        bool valid_d1 = d1 == cpf[9];
        bool valid_d2 = d2 == cpf[10];

        if (isSingleDigit || !valid_d1 || !valid_d2)
            throw new InvalidOperationException("Invalid cpf.");

        return new string([.. cpf.Select(d => (char)(d + '0'))]);
    }

    public static int CalculateDigit(int[] cpf, CpfVerifierDigit vd)
    {
        if (cpf.Length < 9 || cpf.Length > 11)
            throw new ArgumentException("Invalid cpf length.");

        if (!Enum.IsDefined(vd))
            throw new InvalidEnumArgumentException(nameof(vd), (int)vd, typeof(CpfVerifierDigit));

        int factor = 9 + (int)vd;
        int count = 8 + (int)vd;
        int sum = 0;

        for (int i = 0; i < count; i++)
        {
            sum += cpf[i] * factor;
            --factor;
        }

        int digit = sum * 10 % 11 % 10;

        return digit;
    }

    public static Cpf Random
    {
        get
        {
            var cpf = new int[11];

            for (int i = 0; i < 9; i++)
            {
                cpf[i] = RandomNumberGenerator.GetInt32(10);
            }

            cpf[9] = CalculateDigit(cpf, CpfVerifierDigit.First);
            cpf[10] = CalculateDigit(cpf, CpfVerifierDigit.Second);

            return new Cpf(new string([.. cpf.Select(d => (char)('0' + d))]));
        }
    }
}