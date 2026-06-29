using Microsoft.EntityFrameworkCore;
using PetWorld.Application.Products.Abstractions;
using PetWorld.Domain.Products;

namespace PetWorld.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly PetWorldDbContext _dbContext;

    public ProductRepository(PetWorldDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> SearchAsync(string query, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(query))
            return products;

        var terms = query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return products
            .Where(product => terms.All(term =>
                product.Name.Value.Contains(term, StringComparison.OrdinalIgnoreCase)
                || product.Description.Value.Contains(term, StringComparison.OrdinalIgnoreCase)
                || product.Category.DisplayName.Contains(term, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }
}
