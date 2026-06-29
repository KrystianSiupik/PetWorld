using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetWorld.Application.Chat.Abstractions;
using PetWorld.Application.Products.Abstractions;
using PetWorld.Infrastructure.Agents;
using PetWorld.Infrastructure.Persistence;
using PetWorld.Infrastructure.Persistence.Repositories;
using PetWorld.Infrastructure.Persistence.Seeding;

namespace PetWorld.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PetWorldDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));

        services.AddScoped<IChatRepository, ChatRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ProductCatalogTool>();
        services.AddScoped<IWriterAgent, WriterAgent>();
        services.AddScoped<ICriticAgent, CriticAgent>();
        services.AddScoped<ProductCatalogSeeder>();

        return services;
    }
}
