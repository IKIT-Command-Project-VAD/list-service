namespace ShoppingList.List.Web.ShoppingLists;

public class DeleteShoppingList(IMediator mediator) : Endpoint<DeleteShoppingListRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{Id:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteShoppingListRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteShoppingListCommand(req.Id), ct);
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

public record DeleteShoppingListRequest
{
    public Guid Id { get; init; }
}
