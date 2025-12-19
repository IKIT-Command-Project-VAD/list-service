using ShoppingList.List.UseCases.ListItems;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems.Update;

public class UpdateListItem(IMediator mediator) : Endpoint<UpdateListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateListItemRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var updateResult = await mediator.Send(
            new UpdateListItemCommand(
                req.ListId,
                ownerId.Value,
                req.ItemId,
                req.Name,
                req.Quantity,
                req.Unit,
                req.CategoryId,
                req.Price,
                req.Currency,
                req.IsChecked
            ),
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

        var getResult = await mediator.Send(
            new GetListItemQuery(req.ListId, req.ItemId, ownerId.Value),
            ct
        );
        Response = getResult.Value.ToRecord();
    }
}
