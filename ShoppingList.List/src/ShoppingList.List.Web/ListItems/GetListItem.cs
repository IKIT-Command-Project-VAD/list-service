using ShoppingList.List.UseCases.ListItems;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class GetListItem(IMediator mediator) : Endpoint<GetListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(GetListItemRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new GetListItemQuery(req.ListId, req.ItemId, ownerId.Value),
            ct
        );
        if (!result.IsSuccess)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = result.Value.ToRecord();
    }
}

public record GetListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}

