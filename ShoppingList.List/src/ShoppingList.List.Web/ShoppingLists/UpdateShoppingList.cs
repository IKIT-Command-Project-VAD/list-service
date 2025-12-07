using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShoppingLists;

public class UpdateShoppingList(AppDbContext dbContext)
    : Endpoint<UpdateShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{Id:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateShoppingListRequest req, CancellationToken ct)
    {
        var list = await dbContext
            .ShoppingLists.Include(x => x.Items)
            .Include(x => x.ShareLinks)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        list.UpdateName(req.Name);
        await dbContext.SaveChangesAsync(ct);

        Response = list.ToRecord();
    }
}

public record UpdateShoppingListRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

