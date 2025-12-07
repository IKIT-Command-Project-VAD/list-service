using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class UpdateShareLink(AppDbContext dbContext)
    : Endpoint<UpdateShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateShareLinkRequest req, CancellationToken ct)
    {
        var link = await dbContext.ShareLinks.FirstOrDefaultAsync(
            x => x.ListId == req.ListId && x.Id == req.ShareId,
            ct
        );

        if (link is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        link.Update(req.PermissionType, req.ExpiresAt, req.IsActive);
        await dbContext.SaveChangesAsync(ct);

        Response = link.ToRecord();
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

