using ShoppingList.List.UseCases.ListItems;

namespace ShoppingList.List.Web.ListItems;

public class DeleteListItem(IMediator mediator) : Endpoint<DeleteListItemRequest>
{
    public override void Configure()
    {
        Delete("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(DeleteListItemRequest req, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteListItemCommand(req.ListId, req.ItemId), ct);
        if (result.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}

public record DeleteListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
}

