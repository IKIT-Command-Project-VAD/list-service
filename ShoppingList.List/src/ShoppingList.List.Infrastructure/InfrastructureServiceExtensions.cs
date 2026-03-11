using Microsoft.EntityFrameworkCore;
using Npgsql;
using ShoppingList.List.Core.Interfaces;
using ShoppingList.List.Core.Services;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Infrastructure.Data.Queries;
using ShoppingList.List.UseCases.Contributors.List;

namespace ShoppingList.List.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration config,
        ILogger logger
    )
    {
        string? provider = config["DatabaseProvider"] ?? "Postgres";
        string connectionString = config.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        }
        else if (provider.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
        {
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(connectionString));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
        }

        services
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
            .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
            .AddScoped<IDeleteContributorService, DeleteContributorService>();

        logger.LogInformation("{Project} services registered for {Provider}", "Infrastructure", provider);

        return services;
    }
}
