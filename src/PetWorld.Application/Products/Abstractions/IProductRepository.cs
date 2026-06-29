using PetWorld.Domain.Products;

namespace PetWorld.Application.Products.Abstractions;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
}
