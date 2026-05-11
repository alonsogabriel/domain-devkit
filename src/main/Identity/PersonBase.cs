using System.Text;

namespace DomainDevKit.Identity;

public enum Gender
{
    Male,
    Female,
}

public interface IPersonBase
{
    PersonName Name { get; }
    Gender? Gender { get; }
    BirthDate? BirthDate { get; }
}

public abstract class PersonBase<TId, TValue>
    : EntitySoftDelete<TId, TValue>, IPersonBase
    where TId : ObjectId<TValue>
    where TValue : notnull
{
    protected PersonBase() { }

    protected PersonBase(TId id, PersonName name, Gender? gender, BirthDate? birthDate)
    {
        Id = id;
        Name = name;

        if (gender.HasValue && !Enum.IsDefined(gender.Value))
            throw new ArgumentException("Invalid gender value.");

        Gender = gender;
        BirthDate = birthDate;
    }

    public PersonName Name { get; protected set; }
    public Gender? Gender { get; private set; }
    public BirthDate? BirthDate { get; private set; }

    public override string ToString()
    {
        return new StringBuilder()
            .AppendLine($"Name: {Name.FullName}")
            .AppendLine($"Gender: {Gender}")
            .AppendLine($"Birth date: {BirthDate?.Value}")
            .AppendLine($"Age: {BirthDate?.GetAge()}")
            .ToString();
    }
}