using ShoppingList.List.UseCases.ShoppingLists;

namespace ShoppingList.List.Web.ShoppingLists;

public class ListShoppingLists(IMediator mediator) : EndpointWithoutRequest<List<ShoppingListRecord>>
{
    public override void Configure()
    {
        Get("/api/lists");
        Roles("user");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await mediator.Send(new ListShoppingListsQuery(), ct);
        Response = result.Value.Select(l => l.ToRecord()).ToList();
    }
}

