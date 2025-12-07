using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class GetShareLink(AppDbContext dbContext)
    : Endpoint<GetShareLinkRequest, ShareLinkRecord>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetShareLinkRequest req, CancellationToken ct)
    {
        var link = await dbContext
            .ShareLinks.AsNoTracking()
            .FirstOrDefaultAsync(x => x.ListId == req.ListId && x.Id == req.ShareId, ct);

        if (link is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        Response = link.ToRecord();
    }
}

public record GetShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid ShareId { get; init; }
}

