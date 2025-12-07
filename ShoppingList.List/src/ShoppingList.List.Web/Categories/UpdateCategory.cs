using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.Categories;

public class UpdateCategory(AppDbContext dbContext)
    : Endpoint<UpdateCategoryRequest, CategoryRecord>
{
    public override void Configure()
    {
        Put("/api/categories/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCategoryRequest req, CancellationToken ct)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        if (category is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        category.Update(req.Name, req.Icon);
        await dbContext.SaveChangesAsync(ct);

        Response = category.ToRecord();
    }
}

public record UpdateCategoryRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; }
}

