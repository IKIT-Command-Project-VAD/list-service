using ShoppingList.List.UseCases.ListItems;

namespace ShoppingList.List.Web.ListItems.Delete;

public class DeleteListItem(IMediator mediator) : Endpoint<DeleteListItemRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteListItemRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new DeleteListItemCommand(req.ListId, ownerId.Value, req.ItemId),
            ct
        );
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
