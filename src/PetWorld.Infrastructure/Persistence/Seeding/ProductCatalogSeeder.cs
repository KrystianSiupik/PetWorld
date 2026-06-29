using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Products;

namespace PetWorld.Infrastructure.Persistence.Seeding;

public sealed class ProductCatalogSeeder
{
    private readonly PetWorldDbContext _dbContext;

    public ProductCatalogSeeder(PetWorldDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _dbContext.Products.AnyAsync(cancellationToken))
            return;

        var products = new[]
        {
            Product.Create("Karma sucha dla psów dorosłych", ProductCategory.Dogs, "Pełnoporcjowa karma z kurczakiem dla psów ras średnich.", 79.99m, Currency.Pln),
            Product.Create("Gryzak Kong dla psa", ProductCategory.Dogs, "Wytrzymały gryzak na nudę i czyszczenie zębów.", 39.99m, Currency.Pln),
            Product.Create("Karma sucha dla kotów", ProductCategory.Cats, "Pełnoporcjowa karma z łososiem dla kotów dorosłych.", 69.99m, Currency.Pln),
            Product.Create("Drapak dla kota", ProductCategory.Cats, "Słupek z sizalu z platformą do drapania i zabawy.", 129.99m, Currency.Pln),
            Product.Create("Pokarm dla papug", ProductCategory.Birds, "Mieszanka ziaren dla papug małych i średnich.", 24.99m, Currency.Pln),
            Product.Create("Klatka dla małych ptaków", ProductCategory.Birds, "Przestronna klatka z wyposażeniem.", 199.99m, Currency.Pln),
            Product.Create("Pokarm dla ryb akwariowych", ProductCategory.Fish, "Płatki dla ryb słodkowodnych.", 19.99m, Currency.Pln),
            Product.Create("Filtr wewnętrzny do akwarium", ProductCategory.Fish, "Filtr do akwariów do 100 litrów.", 89.99m, Currency.Pln),
            Product.Create("Karma dla królika", ProductCategory.SmallPets, "Granulat z warzywami dla królików i gryzoni.", 29.99m, Currency.Pln),
            Product.Create("Lampa UVB dla gadów", ProductCategory.Reptiles, "Lampa UVB wspierająca zdrowie gadów w terrarium.", 109.99m, Currency.Pln),
        };

        await _dbContext.Products.AddRangeAsync(products, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
