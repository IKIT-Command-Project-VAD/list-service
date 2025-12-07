using ShoppingList.List.Core.ShoppingListAggregate;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class CreateCategory(AppDbContext dbContext) : Endpoint<CreateCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Post("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var category = new Category(req.Name, req.Icon);

        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync(ct);

        Response = category.ToRecord();
    }
}

public record CreateCategoryRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; }
}

