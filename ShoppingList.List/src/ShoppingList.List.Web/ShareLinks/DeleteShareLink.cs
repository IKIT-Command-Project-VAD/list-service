using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;

namespace ShoppingList.List.Web.ShareLinks;

public class DeleteShareLink(AppDbContext dbContext) : Endpoint<DeleteShareLinkRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/share-links/{ShareId:guid}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteShareLinkRequest req, CancellationToken ct)
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

        dbContext.ShareLinks.Remove(link);
        await dbContext.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}

public record DeleteShareLinkRequest
{
    public Guid ListId { get; init; }
    public Guid ShareId { get; init; }
}

