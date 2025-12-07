using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class GetListItem(AppDbContext dbContext) : Endpoint<GetListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetListItemRequest req, CancellationToken ct)
    {
        var item = await dbContext
            .ListItems.AsNoTracking()
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.ListId == req.ListId && i.Id == req.ItemId, ct);

        if (item is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = item.ToRecord();
    }
}

public record GetListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}

