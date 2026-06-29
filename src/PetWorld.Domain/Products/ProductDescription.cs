using PetWorld.Domain.SharedKernel;

namespace PetWorld.Domain.Products;

public sealed record ProductDescription
{
    public const int MaxLength = 1000;

    public string Value { get; }

    private ProductDescription(string value)
    {
        Value = value;
    }

    public static ProductDescription Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("ProductDescription.Empty", "Description must not be empty.");

        if (value.Length > MaxLength)
            throw new DomainException("ProductDescription.TooLong", $"Description must not exceed {MaxLength} characters.");

        return new ProductDescription(value);
    }
}
