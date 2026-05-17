using System.Security.Cryptography;
using DomainDevKit;

namespace DomainDevkit.Tests.Unit.Domain;

public class ObjectIdTests
{
    [Fact(DisplayName = "All ids should be equal")]
    public void AllIdsShouldBeEqual()
    {
        // Given
        var rand = RandomNumberGenerator.GetInt32(int.MaxValue);
        var a = new SomeId(rand);
        var b = new SomeId(a.Value);
        var c = new SomeId();
        var d = c;

        // When

        // Then
        Assert.Equal(a, b);
        Assert.Equal(c, d);
    }

    [Fact(DisplayName = "All ids should be different")]
    public void AllIdsShouldBeDifferent()
    {
        // Given
        var rand = RandomNumberGenerator.GetInt32(2, int.MaxValue);
        var a = new SomeId(rand);
        var b = new SomeId(rand - 1);
        var c = new SomeId();

        // When

        // Then
        Assert.NotEqual(a, b);
        Assert.NotEqual(a, c);
        Assert.NotEqual(b, c);
    }
}

public readonly struct SomeId(int value) : IValueObject<int>
{
    public int Value { get; private init; } = value;
}