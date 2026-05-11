using DomainDevKit.Drawing;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain.Drawing;

public class ColorTests(ITestOutputHelper output)
{
    [Fact]
    public void TestName()
    {
        var color = Color.Random;
        var hexValue = color.ToString();
        var copy = Color.FromHEX(hexValue);

        output.WriteLine(hexValue);
        output.WriteLine(copy.ToString());
        output.WriteLine(Colors.Red.ToString());
        output.WriteLine(Colors.Green.ToString());
        output.WriteLine(Colors.Blue.CopyWith(alpha: 150).ToString());
        output.WriteLine(Color.FromHEX("fab9").ToString());

        Assert.Equal(color, copy);
    }
}