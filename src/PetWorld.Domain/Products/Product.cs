using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed class Product
{
    public Guid Id { get; }
    public ProductName Name { get; }
    public ProductCategory Category { get; }
    public ProductDescription Description { get; }
    public Money Price { get; }

    private Product(Guid id, ProductName name, ProductCategory category, ProductDescription description, Money price)
    {
        Id = id;
        Name = name;
        Category = category;
        Description = description;
        Price = price;
    }

    public static Product Create(string name, ProductCategory category, string description, decimal price, Currency currency)
    {
        if (category is null)
            throw new DomainException("Product.CategoryRequired", "Category is required.");

        var productName = ProductName.Create(name);
        var productDescription = ProductDescription.Create(description);
        var money = Money.Create(price, currency);

        return new Product(Guid.NewGuid(), productName, category, productDescription, money);
    }
}
