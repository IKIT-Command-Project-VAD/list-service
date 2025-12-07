namespace ShoppingList.List.Web.ShoppingLists;

public class CreateShoppingList(IMediator mediator)
    : Endpoint<CreateShoppingListRequest, ShoppingListRecord>
{
    public override void Configure()
    {
        Post("/api/lists");
        Roles("user");
    }

    public override async Task HandleAsync(CreateShoppingListRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var createResult = await mediator.Send(
            new CreateShoppingListCommand(ownerId.Value, req.Name),
            ct
        );
        if (createResult.Status == ResultStatus.Invalid)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var getResult = await mediator.Send(
            new GetShoppingListQuery(createResult.Value, ownerId.Value),
            ct
        );
        if (!getResult.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = getResult.Value.ToRecord();
    }
}

public record CreateShoppingListRequest
{
    public string Name { get; init; } = string.Empty;
}
