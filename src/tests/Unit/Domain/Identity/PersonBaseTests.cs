using DomainDevKit;
using DomainDevKit.Identity;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain.Identity;

public class PersonBaseTests(ITestOutputHelper output)
{
    [Fact]
    public void TestName()
    {
        // Given
        var name = new PersonName("Gabriel", "Willian", "Alonso", null);
        var gender = Gender.Male;
        var birthDate = new BirthDate(DateOnly.Parse("1997-12-08"));
        var person = new Person(name, gender, birthDate);

        // When

        // Then
        output.WriteLine(person.ToString());
    }
}

internal sealed record PersonId(Guid Id) : ObjectId<Guid>(Id)
{
    public static PersonId New() => new(Guid.NewGuid());
}

internal class Person : PersonBase<PersonId, Guid>
{
    public Person(PersonName name, Gender gender, BirthDate birthDate)
        : base(PersonId.New(), name, gender, birthDate)
    {
        
    }
}