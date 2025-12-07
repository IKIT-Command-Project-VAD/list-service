using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using ShoppingList.List.UseCases.ShareLinks;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class CreateShareLink(IMediator mediator)
    : Endpoint<CreateShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Post("/api/lists/{ListId:guid}/share-links");
        Roles("user");
    }

    public override async Task HandleAsync(CreateShareLinkRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var createResult = await mediator.Send(
            new CreateShareLinkCommand(req.ListId, ownerId.Value, req.CreatedBy, req.PermissionType, req.ExpiresAt),
            ct
        );
        if (createResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var getResult = await mediator.Send(
            new GetShareLinkQuery(req.ListId, createResult.Value.Id, ownerId.Value),
            ct
        );
        Response = getResult.Value.ToRecord();
    }
}

public record CreateShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid CreatedBy { get; init; }
    public SharePermissionType PermissionType { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
}

