using Microsoft.EntityFrameworkCore;
using PetWorld.Domain.Chat;
using PetWorld.Domain.Products;

namespace PetWorld.Infrastructure.Persistence;

public sealed class PetWorldDbContext : DbContext
{
    public PetWorldDbContext(DbContextOptions<PetWorldDbContext> options) : base(options)
    {
    }

    public DbSet<ChatInteraction> ChatInteractions => Set<ChatInteraction>();

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetWorldDbContext).Assembly);
    }
}
