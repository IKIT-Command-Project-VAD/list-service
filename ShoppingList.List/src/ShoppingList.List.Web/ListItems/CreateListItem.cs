using ShoppingList.List.UseCases.ListItems;
using ShoppingList.List.Web.ShoppingLists;

namespace ShoppingList.List.Web.ListItems;

public class CreateListItem(IMediator mediator)
    : Endpoint<CreateListItemRequest, ListItemRecord>
{
    public override void Configure()
    {
        Post("/api/lists/{ListId:guid}/items");
        Roles("user");
    }

    public override async Task HandleAsync(CreateListItemRequest req, CancellationToken ct)
    {
        var ownerId = User.GetUserIdAsGuid();
        if (ownerId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var createResult = await mediator.Send(
            new CreateListItemCommand(
                req.ListId,
                ownerId.Value,
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

        if (createResult.Status == ResultStatus.NotFound)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var getResult = await mediator.Send(
            new GetListItemQuery(req.ListId, createResult.Value.Id, ownerId.Value),
            ct
        );

        Response = getResult.Value.ToRecord();
    }
}

public record CreateListItemRequest
{
    public Guid ListId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public string? Unit { get; init; }
    public Guid? CategoryId { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool IsChecked { get; init; }
}

