namespace ShoppingList.List.Web.ShoppingLists;

public class GetShoppingList(IMediator mediator)
    : Endpoint<GetShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(GetShoppingListRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new GetShoppingListQuery(req.Id), ct);
        if (!result.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = result.Value.ToRecord();
    }
}

public record GetShoppingListRequest
{
    public Guid Id { get; init; }
}
