using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PetWorld.Infrastructure.Persistence;

public sealed class PetWorldDbContextFactory : IDesignTimeDbContextFactory<PetWorldDbContext>
{
    public PetWorldDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<PetWorldDbContext>()
            .UseMySql(
                "server=localhost;database=petworld;user=root;password=root",
                new MySqlServerVersion(new Version(8, 0, 0)))
            .Options;

        return new PetWorldDbContext(options);
    }
}
