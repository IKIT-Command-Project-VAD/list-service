using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class ListCategories(AppDbContext dbContext)
    : EndpointWithoutRequest<List<CategoryRecord>>
{
    public override void Configure()
    {
        Get("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categories = await dbContext.Categories.AsNoTracking().ToListAsync(ct);
        Response = categories.Select(c => c.ToRecord()).ToList();
    }
}

