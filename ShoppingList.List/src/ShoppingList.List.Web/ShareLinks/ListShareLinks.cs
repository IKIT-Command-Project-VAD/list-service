using Microsoft.EntityFrameworkCore;
using ShoppingList.List.Infrastructure.Data;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ShareLinks;

public class ListShareLinks(AppDbContext dbContext)
    : Endpoint<ListShareLinksRequest, List<ShareLinkRecord>>
{
    public override void Configure()
    {
        Get("/api/lists/{ListId:guid}/share-links");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ListShareLinksRequest req, CancellationToken ct)
    {
        var links = await dbContext
            .ShareLinks.AsNoTracking()
            .Where(x => x.ListId == req.ListId)
            .ToListAsync(ct);

        Response = links.Select(l => l.ToRecord()).ToList();
    }
}

public record ListShareLinksRequest
{
    public Guid ListId { get; init; }
}

