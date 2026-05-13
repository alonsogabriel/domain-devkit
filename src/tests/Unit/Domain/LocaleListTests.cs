using DomainDevKit;
using Xunit.Abstractions;

namespace DomainDevkit.Tests.Unit.Domain;

public class LocaleListTests(ITestOutputHelper output)
{
    [Fact(DisplayName = "Add should fail when locale exists")]
    public void AddShouldFailWhenLocaleExists()
    {
        // Given
        LocaleList<ProductDescription> list = [];
        var description = new ProductDescription
        {
            Locale = new("pt-br"),
            Value = "Computador de mesa"
        };

        // When
        list.Add(description);

        // Then
        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            list.Add(new()
            {
                Locale = new("pt-BR"),
                Value = "Máquina de lavar roupas"
            });
        });

        output.WriteLine(ex.Message);
    }

    [Fact(DisplayName = "Insert should fail when locale exists")]
    public void InsertShouldFailWhenLocalExists()
    {
        // Given
        LocaleList<ProductDescription> list = [];
        var description = new ProductDescription
        {
            Locale = new("en-US"),
            Value = "Desktop computer"
        };

        // When
        list.Insert(0, description);

        // Then
        var ex = Assert.ThrowsAny<Exception>(() =>
        {
            list.Insert(list.Count, new()
            {
                Locale = new("en-us"),
                Value = "Washing machine"
            });
        });
        output.WriteLine(ex.Message);
    }
}

public class ProductDescription : ILocaleData
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Locale Locale { get; set; }
    public string Value { get; set; }
}