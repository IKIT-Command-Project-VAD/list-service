using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShoppingLists;

public class CreateShoppingList(AppDbContext dbContext)
    : Endpoint<CreateShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Post("/api/lists");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateShoppingListRequest req, CancellationToken ct)
    {
        var list = ShoppingListEntity.Create(req.OwnerId, req.Name);

        dbContext.ShoppingLists.Add(list);
        await dbContext.SaveChangesAsync(ct);

        Response = list.ToRecord();
    }
}

public record CreateShoppingListRequest
{
    public Guid OwnerId { get; init; }
    public string Name { get; init; } = string.Empty;
}

