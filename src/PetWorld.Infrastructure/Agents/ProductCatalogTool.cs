using System.ComponentModel;
using PetWorld.Application.Products.Abstractions;

namespace PetWorld.Infrastructure.Agents;

public sealed class ProductCatalogTool
{
    private readonly IProductRepository _products;

    public ProductCatalogTool(IProductRepository products)
    {
        _products = products;
    }

    [Description("Zwraca pełną listę produktów dostępnych w ofercie sklepu PetWorld.")]
    public async Task<IReadOnlyList<ProductInfo>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await _products.GetAllAsync(cancellationToken);

        return products
            .Select(product => new ProductInfo(
                product.Name.Value,
                product.Category.DisplayName,
                product.Description.Value,
                product.Price.Amount,
                product.Price.Currency.Code))
            .ToList();
    }
}
