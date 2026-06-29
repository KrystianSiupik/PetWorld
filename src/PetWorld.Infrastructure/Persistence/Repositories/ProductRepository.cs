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

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
