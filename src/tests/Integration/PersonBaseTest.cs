using DomainDevKit;
using DomainDevKit.Identity;
using DomainDevKit.EFCore;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Integration;

public class PersonBaseTests(ITestOutputHelper output)
{
    [Fact]
    public void CreateOrder()
    {
        // Given
        using var db = new FakeDb();
        var name = new PersonName("Gabriel", null, "Alonso", null);
        var gender = Gender.Male;
        var birthDate = new BirthDate(DateOnly.Parse("1997-12-08"));
        var customer = new Customer(name, gender, birthDate);
        var order = new Order(customer.Id);

        output.WriteLine(customer.Id.ToString());
        output.WriteLine(customer.ToString());
        output.WriteLine(order.Id.ToString());

        // When
        db.Customers.Add(customer);
        db.Orders.Add(order);
        db.SaveChanges();

        // Then
        var savedOrder = db.Orders.FirstOrDefault(o => o.Id == order.Id);

        Assert.NotNull(savedOrder);
        Assert.NotNull(db.Customers.AsNoTracking().FirstOrDefault(c => c.Id == savedOrder.CustomerId));

        output.WriteLine(savedOrder.Id.ToString());
        output.WriteLine(savedOrder.CustomerId.ToString());
        output.WriteLine(savedOrder.Status.ToString());
    }
}

internal class FakeDb : DbContext
{
    public DbSet<Customer> Customers { get; init; }
    public DbSet<Order> Orders { get; init; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("FakeDb");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Customer>(c =>
            {
                c.MapPersonBase<Customer, CustomerId, Guid>(v => new CustomerId(v), id => id.ValueGeneratedNever());

            })
            .MapEnum<OrderStatus>()
            .Entity<Order>(o =>
            {
                o.MapEntity<Order, OrderId, Guid>(v => new OrderId(v), id => id.ValueGeneratedNever());

                o.HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey(o => o.CustomerId)
                    .IsRequired();

                o.HasEnum<Order, OrderStatus>(x => x.StatusId);
            });
    }
}

internal sealed record CustomerId(Guid Value) : ObjectId<Guid>(Value)
{
    public CustomerId() : this(Guid.NewGuid()) { }
}

internal class Customer : PersonBase<CustomerId, Guid>
{
    private Customer() { }
    public Customer(PersonName name, Gender gender, BirthDate birthDate)
        : base(new CustomerId(), name, gender, birthDate) { }
}

internal sealed record OrderId(Guid Value) : ObjectId<Guid>(Value)
{
    public OrderId() : this(Guid.NewGuid()) { }
}

internal class Order : EntitySoftDelete<OrderId, Guid>
{
    private Order() { }

    public Order(CustomerId customerId) : base(new OrderId())
    {
        CustomerId = customerId;
        StatusId = OrderStatus.New.Id;
    }

    public CustomerId CustomerId { get; set; }
    public int StatusId { get; private set; }
    public OrderStatus Status => OrderStatus.ById(StatusId);
}

internal record OrderStatus : EnumEntity<OrderStatus>
{
    public static readonly OrderStatus New = new(1, nameof(New));
    public static readonly OrderStatus Complete = new(2, nameof(Complete));
    public static readonly OrderStatus Canceled = new(3, nameof(Canceled));
    private OrderStatus(int id, string name) : base(id, name) { }
}