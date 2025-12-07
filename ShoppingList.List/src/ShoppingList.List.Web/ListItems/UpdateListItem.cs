using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class UpdateListItem(AppDbContext dbContext)
    : Endpoint<UpdateListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateListItemRequest req, CancellationToken ct)
    {
        var item = await dbContext
            .ListItems.Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.ListId == req.ListId && i.Id == req.ItemId, ct);

        if (item is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        item.Update(
            req.Name,
            req.Quantity,
            req.Unit,
            req.CategoryId,
            req.Price,
            req.Currency,
            req.IsChecked
        );

        await dbContext.SaveChangesAsync(ct);

        Response = item.ToRecord();
    }
}

public record UpdateListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool IsChecked { get; init; }
}

