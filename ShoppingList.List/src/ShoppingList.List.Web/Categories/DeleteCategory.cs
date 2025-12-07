using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.Categories;

public class DeleteCategory(AppDbContext dbContext) : Endpoint<DeleteCategoryRequest>
{
    public override void Configure()
    {
        Delete("/api/categories/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCategoryRequest req, CancellationToken ct)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == req.Id, ct);
        if (category is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

public record DeleteCategoryRequest
{
    public Guid Id { get; init; }
}

