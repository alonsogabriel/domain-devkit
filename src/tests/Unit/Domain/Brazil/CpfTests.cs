using DomainDevKit.Brazil;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain.Brazil;

public class CpfTests(ITestOutputHelper output)
{
    [Fact]
    public void ListCpfs()
    {
        // Given
        var cpfs = new Cpf[1];

        for (int i = 0; i < cpfs.Length; i++)
        {
            cpfs[i] = Cpf.Random;

            output.WriteLine(cpfs[i].ToString());
        }

        Assert.All(cpfs, cpf => Assert.Equal(cpf, new Cpf(cpf.Formatted)));

        // When

        // Then
    }
}