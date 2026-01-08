using MediatR;
using ShoppingList.List.UseCases.ListItems;

namespace ShoppingList.List.Web.ListItems.Delete;

public class DeleteListItems(IMediator _mediator) : Endpoint<DeleteListItemsRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/items");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteListItemsRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await _mediator.Send(
            new DeleteListItemsCommand(req.ListId, ownerId.Value, req.ListItemIds),
            ct
        );

        await SendNoContentAsync(ct);
    }
}
