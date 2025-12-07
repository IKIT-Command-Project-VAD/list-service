using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class ListListItems(AppDbContext dbContext)
    : Endpoint<ListListItemsRequest, List<ListItemRecord>>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListListItemsRequest req, CancellationToken ct)
    {
        var items = await dbContext
            .ListItems.AsNoTracking()
            .Where(i => i.ListId == req.ListId)
            .Include(i => i.Category)
            .ToListAsync(ct);

        Response = items.Select(i => i.ToRecord()).ToList();
    }
}

public record ListListItemsRequest
{
    public Guid ListId { get; init; }
}

