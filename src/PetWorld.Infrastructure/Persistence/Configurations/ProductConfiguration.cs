using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetWorld.Domain.Products;

namespace PetWorld.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(product => product.Id);

        builder.Property(product => product.Name)
            .HasConversion(
                name => name.Value,
                value => ProductName.Create(value))
            .HasColumnName("Name")
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasConversion(
                description => description.Value,
                value => ProductDescription.Create(value))
            .HasColumnName("Description")
            .HasMaxLength(ProductDescription.MaxLength)
            .IsRequired();

        builder.Property(product => product.Category)
            .HasConversion(
                category => category.Name,
                name => ProductCategory.FromName(name)!)
            .HasColumnName("Category")
            .HasMaxLength(50)
            .IsRequired();

        builder.OwnsOne(product => product.Price, price =>
        {
            price.Property(money => money.Amount)
                .HasColumnName("PriceAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            price.Property(money => money.Currency)
                .HasConversion(
                    currency => currency.Name,
                    name => Currency.FromName(name)!)
                .HasColumnName("PriceCurrency")
                .HasMaxLength(10)
                .IsRequired();
        });
    }
}
