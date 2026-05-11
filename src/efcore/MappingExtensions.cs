using System.Linq.Expressions;
using DomainDevKit;
using DomainDevKit.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainDevKit.EFCore;

public static class MappingExtensions
{
    public static void MapEntity<E, I, V>(this EntityTypeBuilder<E> builder,
        Expression<Func<V, I>> idConversion, Action<PropertyBuilder<I>>? idBuilder = null)
        where E : Entity<I, V>
        where I : ObjectId<V>
        where V : notnull
    {
        builder.HasKey(e => e.Id);

        var id = builder.Property(e => e.Id)
            .HasConversion(id => id.Value, idConversion);

        idBuilder?.Invoke(id);
    }

    public static void MapEntityTimestamps<E, I, V>(this EntityTypeBuilder<E> builder,
        Expression<Func<V, I>> idConversion, Action<PropertyBuilder<I>>? idBuilder = null)
        where E : EntityTimestamps<I, V>
        where I : ObjectId<V>
        where V : notnull
    {
        builder.MapEntity(idConversion, idBuilder);
        builder.Property(e => e.CreatedAt)
            .IsRequired();
        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
    }

    public static void MapEntitySoftDelete<E, I, V>(this EntityTypeBuilder<E> builder,
       Expression<Func<V, I>> idConversion, Action<PropertyBuilder<I>>? idBuilder = null)
       where E : EntitySoftDelete<I, V>
       where I : ObjectId<V>
       where V : notnull
    {
        builder.MapEntityTimestamps(idConversion, idBuilder);
        builder.Property(x => x.DeletedAt)
            .IsRequired(false);
    }

    public static void MapPersonBase<T, I, V>(this EntityTypeBuilder<T> builder, Expression<Func<V, I>> idConversion, Action<PropertyBuilder<I>>? idBuilder = null)
        where T : PersonBase<I, V>
        where I : ObjectId<V>
        where V : notnull
    {
        builder.MapEntity(idConversion, idBuilder);

        builder.MapPersonName(p => p.Name);
        builder.MapGender(p => p.Gender, false);
        builder.MapBirthDate(p => p.BirthDate);
    }

    public static void MapPersonName<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, PersonName?>> name)
        where T : class
    {
        builder.OwnsOne(name, x =>
        {
            x.Property(n => n.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName(nameof(PersonName.FirstName));

            x.Property(n => n.MiddleName)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName(nameof(PersonName.MiddleName));

            x.Property(n => n.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName(nameof(PersonName.LastName));

            x.Property(n => n.Suffix)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName("NameSuffix");

            x.Ignore(n => n.FullName);
        });
    }

    public static void MapGender<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, Gender?>> gender, bool required = true)
        where T : class
    {
        builder.Property(gender)
            .IsRequired(required)
            .HasMaxLength(1)
            .IsFixedLength()
            .HasConversion(
                g => g.HasValue ? Enum.GetName(g.Value)![0] : (char?)null,
                v => Enum.GetValues<Gender>().FirstOrDefault(g => Enum.GetName(g)![0] == v));
    }

    public static void MapBirthDate<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, BirthDate?>> date)
        where T : class
    {
        builder.OwnsOne(date, x =>
        {
            x.Property(d => d.Value)
                .IsRequired();
        });
    }

    public static ModelBuilder MapEnum<T>(this ModelBuilder builder)
        where T : EnumEntity<T>
    {
        return builder.Entity<T>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.Id)
                .ValueGeneratedNever();

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            e.HasIndex(x => x.Name)
                .IsUnique();

            e.HasData(EnumEntity<T>.Values);
        });
    }

    public static void HasEnum<T, E>(this EntityTypeBuilder<T> builder, Expression<Func<T, object?>> fk, bool required = true)
        where T : class
        where E : EnumEntity<E>
    {
        builder.HasOne<E>()
            .WithMany()
            .HasForeignKey(fk)
            .IsRequired(required);
    }
}