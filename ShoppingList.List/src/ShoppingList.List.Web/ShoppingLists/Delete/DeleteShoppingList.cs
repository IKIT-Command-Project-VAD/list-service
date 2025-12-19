namespace ShoppingList.List.Web.ShoppingLists.Delete;

public class DeleteShoppingList(IMediator mediator) : Endpoint<DeleteShoppingListRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteShoppingListRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new DeleteShoppingListCommand(req.Id, ownerId.Value), ct);
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        if (result.Status == ResultStatus.Invalid)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
