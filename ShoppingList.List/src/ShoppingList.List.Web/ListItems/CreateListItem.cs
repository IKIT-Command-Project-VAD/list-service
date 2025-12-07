using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class CreateListItem(AppDbContext dbContext)
    : Endpoint<CreateListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Post("/api/lists/{ListId:guid}/items");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateListItemRequest req, CancellationToken ct)
    {
        var list = await dbContext
            .ShoppingLists.Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == req.ListId, ct);

        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var item = list.AddItem(
            name: req.Name,
            quantity: req.Quantity,
            unit: req.Unit,
            categoryId: req.CategoryId,
            price: req.Price,
            currency: req.Currency,
            isChecked: req.IsChecked
        );

        await dbContext.SaveChangesAsync(ct);

        // Load category for response if provided
        if (req.CategoryId.HasValue)
        {
            item = await dbContext.ListItems.Include(i => i.Category).FirstAsync(
                i => i.Id == item.Id,
                ct
            );
        }

        Response = item.ToRecord();
    }
}

public record CreateListItemRequest
{
    public Guid ListId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool IsChecked { get; init; }
}

