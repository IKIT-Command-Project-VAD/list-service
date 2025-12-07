using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class GetCategory(AppDbContext dbContext) : Endpoint<GetCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Get("/api/categories/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetCategoryRequest req, CancellationToken ct)
    {
        var category = await dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(
            c => c.Id == req.Id,
            ct
        );

        if (category is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = category.ToRecord();
    }
}

public record GetCategoryRequest
{
    public Guid Id { get; init; }
}

