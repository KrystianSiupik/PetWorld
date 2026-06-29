using System.ComponentModel;
using PetWorld.Application.Products.Abstractions;

namespace PetWorld.Infrastructure.Agents;

public sealed class ProductSearchTool
{
    private readonly IProductRepository _products;

    public ProductSearchTool(IProductRepository products)
    {
        _products = products;
    }

    [Description("Wyszukuje produkty z oferty sklepu PetWorld pasujące do podanego zapytania.")]
    public async Task<IReadOnlyList<ProductSearchResult>> SearchProducts(
        [Description("Słowa kluczowe opisujące szukany produkt, np. 'karma dla szczeniąt'.")] string query,
        CancellationToken cancellationToken)
    {
        var products = await _products.SearchAsync(query, cancellationToken);

        return products
            .Select(product => new ProductSearchResult(
                product.Name.Value,
                product.Category.DisplayName,
                product.Description.Value,
                product.Price.Amount,
                product.Price.Currency.Code))
            .ToList();
    }
}
