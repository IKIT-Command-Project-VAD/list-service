using ShoppingList.List.Core.ContributorAggregate;
using ShoppingList.List.Core.ShoppingListAggregate;

namespace ShoppingList.List.Infrastructure.Data;

public static class SeedData
{
    public static readonly Contributor Contributor1 = new("Ardalis");
    public static readonly Contributor Contributor2 = new("Snowfrog");

    public static readonly Category[] DefaultCategories =
    [
        new("Овощи"),
        new("Фрукты"),
        new("Молочные продукты"),
        new("Мясо"),
        new("Рыба и морепродукты"),
        new("Хлеб и выпечка"),
        new("Крупы и макароны"),
        new("Бакалея и специи"),
        new("Напитки"),
        new("Сладости и десерты"),
        new("Закуски"),
        new("Заморозка"),
        new("Консервы"),
        new("Соусы и приправы"),
        new("Для дома"),
        new("Бытовая химия"),
        new("Гигиена"),
        new("Детские товары"),
        new("Зоотовары"),
        new("Аптека / здоровье")
    ];

    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        await PopulateContributorsAsync(dbContext);
        await PopulateCategoriesAsync(dbContext);
    }

    private static async Task PopulateContributorsAsync(AppDbContext dbContext)
    {
        if (await dbContext.Contributors.AnyAsync())
            return;

        dbContext.Contributors.AddRange([Contributor1, Contributor2]);
        await dbContext.SaveChangesAsync();
    }

    private static async Task PopulateCategoriesAsync(AppDbContext dbContext)
    {
        if (await dbContext.Categories.AnyAsync())
            return;

        dbContext.Categories.AddRange(DefaultCategories);
        await dbContext.SaveChangesAsync();
    }
}
