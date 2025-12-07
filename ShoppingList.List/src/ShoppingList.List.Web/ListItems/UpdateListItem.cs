using ShoppingList.List.UseCases.ListItems;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class UpdateListItem(IMediator mediator)
    : Endpoint<UpdateListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Put("/api/lists/{ListId:guid}/items/{ItemId:guid}");
        Roles("user");
    }

    public override async Task HandleAsync(UpdateListItemRequest req, CancellationToken ct)
    {
        var updateResult = await mediator.Send(
            new UpdateListItemCommand(
                req.ListId,
                req.ItemId,
                req.Name,
                req.Quantity,
                req.Unit,
                req.CategoryId,
                req.Price,
                req.Currency,
                req.IsChecked
            ),
            ct
        );

        if (updateResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        if (updateResult.Status == ResultStatus.Invalid)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var getResult = await mediator.Send(new GetListItemQuery(req.ListId, req.ItemId), ct);
        Response = getResult.Value.ToRecord();
    }
}

public record UpdateListItemRequest
{
    public Guid ListId { get; init; }
    public Guid ItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool IsChecked { get; init; }
}

