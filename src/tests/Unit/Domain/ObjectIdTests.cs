using System.Net.Sockets;
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
        var d = new SomeId();
    
        // When
    
        // Then
        Assert.NotEqual(a, b);
        Assert.NotEqual(a, c);
        Assert.NotEqual(a, d);
        Assert.NotEqual(b, c);
        Assert.NotEqual(b, d);
        Assert.NotEqual(c, d);
    }
}

public class SomeId : ObjectId<int>
{
    public SomeId() { }
    public SomeId(int value) : base(value) { }
}