using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed record ProductName
{
    public const int MaxLength = 100;

    public string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public static ProductName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("ProductName.Empty", "Name must not be empty.");

        if (value.Length > MaxLength)
            throw new DomainException("ProductName.TooLong", $"Name must not exceed {MaxLength} characters.");

        return new ProductName(value);
    }
}
