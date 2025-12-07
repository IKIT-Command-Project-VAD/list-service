using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShoppingLists;

public class GetShoppingList(AppDbContext dbContext) : Endpoint<GetShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetShoppingListRequest req, CancellationToken ct)
    {
        var list = await dbContext
            .ShoppingLists.AsNoTracking()
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = list.ToRecord();
    }
}

public record GetShoppingListRequest
{
    public Guid Id { get; init; }
}

