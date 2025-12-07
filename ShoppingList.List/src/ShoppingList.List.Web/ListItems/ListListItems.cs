using ShoppingList.List.UseCases.ListItems;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class ListListItems(IMediator mediator)
    : Endpoint<ListListItemsRequest, List<ListItemRecord>>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/items");
        Roles("user");
    }

    public override async Task HandleAsync(ListListItemsRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new ListListItemsQuery(req.ListId, ownerId.Value), ct);
        Response = result.Value.Select(i => i.ToRecord()).ToList();
    }
}

public record ListListItemsRequest
{
    public Guid ListId { get; init; }
}

