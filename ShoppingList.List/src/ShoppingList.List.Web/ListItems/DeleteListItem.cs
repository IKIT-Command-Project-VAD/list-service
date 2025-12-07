using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ListItems;

public class DeleteListItem(AppDbContext dbContext) : Endpoint<DeleteListItemRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteListItemRequest req, CancellationToken ct)
    {
        var item = await dbContext
            .ListItems.FirstOrDefaultAsync(i => i.ListId == req.ListId && i.Id == req.ItemId, ct);

        if (item is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        item.SoftDelete();
        await dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

public record DeleteListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}

