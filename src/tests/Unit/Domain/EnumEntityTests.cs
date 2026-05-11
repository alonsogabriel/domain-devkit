using DomainDevKit;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain;

public class EnumEntityTests(ITestOutputHelper output)
{
    [Fact(DisplayName = "Values should return only valid ones")]
    public void ValuesShouldReturnOnlyValidOnes()
    {
        var values = Tier.Values;
        var length = values.Length;

        Assert.Contains(Tier.S, values);
        Assert.Contains(Tier.A, values);
        Assert.Contains(Tier.B, values);
        Assert.Contains(Tier.C, values);
        Assert.NotEmpty(values);
        Assert.Equal(4, length);
    }

    [Fact(DisplayName = "By id should return successfully")]
    public void ByIdShouldReturnSuccessfully()
    {
        Assert.Equal(Tier.S, Tier.ById(1));
        Assert.Equal(Tier.A, Tier.ById(2));
        Assert.Equal(Tier.B, Tier.ById(3));
        Assert.Equal(Tier.C, Tier.ById(4));
    }

    [Fact(DisplayName = "By id should fail when id does not exist")]
    public void ByIdShouldFailWhenIdDoesNotExist()
    {
        int[] ids = [-1, 0, 5, 6, 7, 8, 9];

        Assert.All(ids, id =>
        {
            var ex = Assert.ThrowsAny<Exception>(() => Tier.ById(id));
            output.WriteLine(ex.Message);
        });
    }

    [Fact(DisplayName = "By name should return successfully")]
    public void ByNameShouldReturnSuccessfully()
    {
        Assert.Equal(Tier.S, Tier.ByName("S"));
        Assert.Equal(Tier.A, Tier.ByName("A"));
        Assert.Equal(Tier.B, Tier.ByName("B"));
        Assert.Equal(Tier.C, Tier.ByName("C"));
    }

    [Fact(DisplayName = "By name should fail when name does not exist")]
    public void ByNameShouldFailWhenNameDoesNotExist()
    {
        string[] names = ["S+", "A+", "Z", "G", "H"];

        Assert.All(names, name =>
        {
            var ex = Assert.ThrowsAny<Exception>(() => Tier.ByName(name));
            output.WriteLine(ex.Message);
        });
    }
}

internal sealed record Tier : EnumEntity<Tier>
{
    public static readonly Tier S = new(1, nameof(S));
    public static readonly Tier A = new(2, nameof(A));
    public static readonly Tier B = new(3, nameof(B));
    public static readonly Tier C = new(4, nameof(C));
    private static readonly Tier D = new(5, nameof(D));
    // This (obviously) causes a stack overflow by an endless recursion
    // public readonly Tier E = new(6, nameof(E));
    public static Tier F = new(7, nameof(F));
    private Tier(int id, string name) : base(id, name) { }
}