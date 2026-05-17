using System.Linq.Expressions;
using DomainDevKit.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainDevKit.EFCore;

public static class MappingExtensions
{
    public static void MapEntity<E, I>(this EntityTypeBuilder<E> builder)
        where E : Entity<I>
        where I : notnull
    {
        builder.HasKey(e => e.Id);
    }

    public static void MapEntityTimestamps<E, I>(this EntityTypeBuilder<E> builder)
        where E : EntityTimestamps<I>
        where I : notnull
    {
        builder.MapEntity<E, I>();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);
    }

    public static void MapEntitySoftDelete<E, I>(this EntityTypeBuilder<E> builder)
       where E : EntitySoftDelete<I>
       where I : notnull
    {
        builder.MapEntityTimestamps<E, I>();

        builder.Property(x => x.DeletedAt)
            .IsRequired(false);

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }

    public static void MapPersonBase<E, I>(this EntityTypeBuilder<E> builder)
        where E : PersonBase<I>
        where I : notnull
    {
        builder.MapEntity<E, I>();

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

    public static EntityTypeBuilder<T> MapLocaleData<T>(this EntityTypeBuilder<T> builder, bool unique = true)
        where T : class, ILocaleData
    {
        return builder.OwnsOne(e => e.Locale, x =>
        {
            x.Property(l => l.Value)
                .HasMaxLength(5)
                .IsRequired();

            if (unique)
            {
                x.HasIndex(l => l.Value)
                    .IsUnique();
            }
        });
    }
}