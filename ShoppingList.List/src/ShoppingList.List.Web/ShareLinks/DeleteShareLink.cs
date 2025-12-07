using ShoppingList.List.UseCases.ShareLinks;

namespace ShoppingList.List.Web.ShareLinks;

public class DeleteShareLink(IMediator mediator) : Endpoint<DeleteShareLinkRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteShareLinkRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await mediator.Send(
            new DeleteShareLinkCommand(req.ListId, req.ShareId, ownerId.Value),
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

public record DeleteShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid ShareId { get; init; }
}

