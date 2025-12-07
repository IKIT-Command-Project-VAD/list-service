using ShoppingList.List.Core.Interfaces;
using ShoppingList.List.Core.Services;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Infrastructure.Data.Queries;
using ShoppingList.List.UseCases.Contributors.List;

namespace ShoppingList.List.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager config,
        ILogger logger
    )
    {
        string connectionString = Guard.Against.NullOrEmpty(
            config.GetConnectionString("DefaultConnection")
        );
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services
            .AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
            .AddScoped<IListContributorsQueryService, ListContributorsQueryService>()
            .AddScoped<IDeleteContributorService, DeleteContributorService>();

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }
}
