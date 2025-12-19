namespace ShoppingList.List.Web.ShoppingLists.Update;

public class UpdateShoppingList(IMediator mediator)
    : Endpoint<UpdateShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateShoppingListRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var updateResult = await mediator.Send(
            new UpdateShoppingListCommand(req.Id, ownerId.Value, req.Name),
            ct
        );
        if (updateResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        if (updateResult.Status == ResultStatus.Invalid)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var getResult = await mediator.Send(new GetShoppingListQuery(req.Id, ownerId.Value), ct);
        if (!getResult.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = getResult.Value.ToRecord();
    }
}
