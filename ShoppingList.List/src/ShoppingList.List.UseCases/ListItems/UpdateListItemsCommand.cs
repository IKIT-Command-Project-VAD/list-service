using Microsoft.Extensions.Logging;
using ShoppingList.List.Core.ShoppingListAggregate.Specifications;

namespace ShoppingList.List.UseCases.ListItems;

public record UpdateListItemsCommand(
    Guid ListId,
    Guid OwnerId,
    Guid[] ListItemIds,
    bool IsChecked
) : ICommand<Result>;

public sealed class UpdateListItemsHandler(
    IRepository<ListItem> itemRepo,
    ILogger<UpdateListItemsHandler> logger
) : ICommandHandler<UpdateListItemsCommand, Result>
{
    public async Task<Result> Handle(
        UpdateListItemsCommand request,
        CancellationToken cancellationToken
    )
    {
        var itemsToUpdate = new List<ListItem>();

        foreach (var itemId in request.ListItemIds)
        {
            var spec = new ListItemByIdSpec(request.ListId, itemId);
            var item = await itemRepo.FirstOrDefaultAsync(spec, cancellationToken);
            if (item is null)
            {
                logger.LogWarning(
                    "Item with ID {ItemId} not found in list {ListId}, skipping",
                    itemId,
                    request.ListId
                );
                continue;
            }

            if (item.List?.OwnerId != request.OwnerId)
            {
                logger.LogWarning(
                    "Item with ID {ItemId} does not belong to owner {OwnerId}, skipping",
                    itemId,
                    request.OwnerId
                );
                continue;
            }

            itemsToUpdate.Add(item);
        }

        if (itemsToUpdate.Count == 0)
        {
            return Result.Success();
        }

        foreach (var item in itemsToUpdate)
        {
            item.ToggleChecked(request.IsChecked);
        }

        await itemRepo.UpdateRangeAsync(itemsToUpdate, cancellationToken);

        return Result.Success();
    }
}
