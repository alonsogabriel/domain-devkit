using DomainDevKit.Identity;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain.Identity;

public class PersonNameTests(ITestOutputHelper output)
{

    [Fact]
    public void InstantiateName()
    {
        // Given
    
        var name = new PersonName("Gabriel", "Willian", "Alonso", null);

        // When
    
        // Then

        output.WriteLine(name.ToString());
    }
}