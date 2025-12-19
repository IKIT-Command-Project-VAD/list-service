namespace ShoppingList.List.Web.ShoppingLists.Read;

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
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new GetShoppingListQuery(req.Id, ownerId.Value), ct);
        if (!result.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = result.Value.ToRecord();
    }
}
