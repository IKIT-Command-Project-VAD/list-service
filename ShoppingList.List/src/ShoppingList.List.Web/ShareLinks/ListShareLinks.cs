using ShoppingList.List.UseCases.ShareLinks;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class ListShareLinks(IMediator mediator)
    : Endpoint<ListShareLinksRequest, List<ShareLinkRecord>>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/share-links");
        Roles("user");
    }

    public override async Task HandleAsync(ListShareLinksRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(new ListShareLinksQuery(req.ListId, ownerId.Value), ct);
        Response = result.Value.Select(l => l.ToRecord()).ToList();
    }
}

public record ListShareLinksRequest
{
    public Guid ListId { get; init; }
}

