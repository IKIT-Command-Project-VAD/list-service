using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShoppingLists;

public class ListShoppingLists(AppDbContext dbContext)
    : EndpointWithoutRequest<List<ShoppingListRecord>>
{
    public override void Configure()
    {
        Get("/api/lists");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var lists = await dbContext
            .ShoppingLists.AsNoTracking()
            .Include(x => x.Items).ThenInclude(i => i.Category)
            .Include(x => x.ShareLinks)
            .ToListAsync(ct);

        Response = lists.Select(l => l.ToRecord()).ToList();
    }
}

