using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShoppingLists;

public class DeleteShoppingList(AppDbContext dbContext) : Endpoint<DeleteShoppingListRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteShoppingListRequest req, CancellationToken ct)
    {
        var list = await dbContext.ShoppingLists.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        list.SoftDelete();
        await dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

public record DeleteShoppingListRequest
{
    public Guid Id { get; init; }
}

