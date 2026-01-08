using MediatR;
using ShoppingList.List.UseCases.ListItems;

namespace ShoppingList.List.Web.ListItems.Update;

public class UpdateListItems(IMediator _mediator) : Endpoint<UpdateListItemsRequest>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/items");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateListItemsRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await _mediator.Send(
            new UpdateListItemsCommand(req.ListId, ownerId.Value, req.ListItemIds, req.IsChecked),
            ct
        );

        await SendNoContentAsync(ct);
    }
}
