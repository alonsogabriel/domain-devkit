using DomainDevKit;
using DomainDevKit.Identity;
using DomainDevKit.EFCore;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using DomainDevkit.Tests.Unit.Domain;

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
        var savedOrder = db.Orders.FirstOrDefault(o => o.Id.Equals(order.Id));
        var savedCustomer = db.Customers.FirstOrDefault(c => c.Id.Equals(customer.Id));
        savedCustomer?.ChangeName(new PersonName("Rapha", null, "Alonso", null));

        Assert.NotNull(savedOrder);
        Assert.NotNull(db.Customers.AsNoTracking().FirstOrDefault(c => c.Id.Equals(savedOrder.CustomerId)));

        output.WriteLine(savedOrder.Id.ToString());
        output.WriteLine(savedOrder.CustomerId.ToString());
        output.WriteLine(savedOrder.Status.ToString());
    }

    [Fact(DisplayName = "Add product successfully")]
    public async Task AddProductSuccessfully()
    {
        // Given
        using var db = new FakeDb();
        var product = new Product();

        // When
        product.Descriptions.Add(new()
        {
            Locale = new("pt-BR"),
            Value = "Máquina de lavar roupas"
        });

        product.Descriptions.Add(new()
        {
            Locale = new("en-US"),
            Value = "Washing machine"
        });

        // Then
        db.Products.Add(product);
        db.SaveChanges();
        var savedProduct = db.Products
            .AsNoTracking()
            .Include(p => p.Descriptions)
            .FirstOrDefault(p => p.Id == product.Id);

        Assert.NotNull(savedProduct);

        foreach (var desc in savedProduct.Descriptions)
        {
            output.WriteLine($"{desc.Locale.Value}: {desc.Value}");
        }
    }
}

internal class FakeDb : DbContext
{
    public DbSet<Product> Products { get; init; }
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
                c.MapPersonBase<Customer, CustomerId>();

                c.Property(x => x.Id)
                    .HasConversion(id => id.Value, v => new CustomerId(v));
            })
            .MapEnum<OrderStatus>()
            .Entity<Order>(o =>
            {
                o.MapEntity<Order, OrderId>();

                o.Property(x => x.Id)
                    .HasConversion(id => id.Value, v => new OrderId(v));

                o.HasOne<Customer>()
                    .WithMany()
                    .HasForeignKey(o => o.CustomerId)
                    .IsRequired();

                o.HasEnum<Order, OrderStatus>(x => x.StatusId);
            })
            .Entity<Product>(p =>
            {
                p.HasMany(x => x.Descriptions)
                    .WithOne()
                    .HasForeignKey(d => d.ProductId)
                    .IsRequired();
            })
            .Entity<ProductDescription>(e =>
            {
                e.MapLocaleData();
                e.Property(x => x.Value)
                    .HasMaxLength(50)
                    .IsRequired();
            });
    }
}

internal readonly struct CustomerId(Guid value) : IValueObject<Guid>
{
    public CustomerId() : this(Guid.NewGuid()) { }
    public Guid Value { get; private init; } = value;
}

internal class Customer : PersonBase<CustomerId>
{
    private Customer() { }
    public Customer(PersonName name, Gender gender, BirthDate birthDate)
        : base(new CustomerId(), name, gender, birthDate) { }

    public void ChangeName(PersonName name)
    {
        Name = name;
    }
}

internal readonly struct OrderId(Guid value) : IValueObject<Guid>
{
    public OrderId() : this(Guid.NewGuid()) { }

    public Guid Value { get; private init; } = value;
}

internal class Order : EntitySoftDelete<OrderId>
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

public class Product
{
    public int Id { get; set; }
    public List<ProductDescription> Descriptions { get; init; } = [];
}

internal record OrderStatus : EnumEntity<OrderStatus>
{
    public static readonly OrderStatus New = new(1, nameof(New));
    public static readonly OrderStatus Complete = new(2, nameof(Complete));
    public static readonly OrderStatus Canceled = new(3, nameof(Canceled));
    private OrderStatus(int id, string name) : base(id, name) { }
}