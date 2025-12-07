using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Core.ShoppingListAggregate.Enums;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class CreateShareLink(AppDbContext dbContext)
    : Endpoint<CreateShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Post("/api/lists/{ListId:guid}/share-links");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateShareLinkRequest req, CancellationToken ct)
    {
        var list = await dbContext
            .ShoppingLists.Include(x => x.ShareLinks)
            .FirstOrDefaultAsync(x => x.Id == req.ListId, ct);

        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var link = list.AddShareLink(req.CreatedBy, req.PermissionType, req.ExpiresAt);

        await dbContext.SaveChangesAsync(ct);
        Response = link.ToRecord();
    }
}

public record CreateShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid CreatedBy { get; init; }
    public SharePermissionType PermissionType { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
}

