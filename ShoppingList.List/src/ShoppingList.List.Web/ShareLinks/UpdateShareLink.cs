using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using ShoppingList.List.UseCases.ShareLinks;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class UpdateShareLink(IMediator mediator) : Endpoint<UpdateShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateShareLinkRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var updateResult = await mediator.Send(
            new UpdateShareLinkCommand(
                req.ListId,
                req.ShareId,
                ownerId.Value,
                req.PermissionType,
                req.ExpiresAt,
                req.IsActive
            ),
            ct
        );

        if (updateResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var getResult = await mediator.Send(
            new GetShareLinkQuery(req.ListId, req.ShareId, ownerId.Value),
            ct
        );
        Response = getResult.Value.ToRecord();
    }
}

public record UpdateShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid ShareId { get; init; }
    public SharePermissionType PermissionType { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
    public bool IsActive { get; init; }
}
