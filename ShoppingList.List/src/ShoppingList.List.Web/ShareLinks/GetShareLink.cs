using ShoppingList.List.UseCases.ShareLinks;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class GetShareLink(IMediator mediator)
    : Endpoint<GetShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(GetShareLinkRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new GetShareLinkQuery(req.ListId, req.ShareId, ownerId.Value),
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

public record GetShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid ShareId { get; init; }
}

