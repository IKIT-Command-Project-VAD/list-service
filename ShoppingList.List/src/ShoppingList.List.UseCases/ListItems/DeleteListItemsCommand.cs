using Microsoft.Extensions.Logging;
using ShoppingList.List.Core.ShoppingListAggregate.Specifications;

namespace ShoppingList.List.UseCases.ListItems;

public record DeleteListItemsCommand(Guid ListId, Guid OwnerId, Guid[] ListItemIds) : ICommand<Result>;

public sealed class DeleteListItemsHandler(
    IRepository<ListItem> itemRepo,
    ILogger<DeleteListItemsHandler> logger
) : ICommandHandler<DeleteListItemsCommand, Result>
{
    public async Task<Result> Handle(
        DeleteListItemsCommand request,
        CancellationToken cancellationToken
    )
    {
        var itemsToDelete = new List<ListItem>();

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

            itemsToDelete.Add(item);
        }

        if (itemsToDelete.Count == 0)
        {
            return Result.Success();
        }

        foreach (var item in itemsToDelete)
        {
            item.SoftDelete();
        }

        await itemRepo.UpdateRangeAsync(itemsToDelete, cancellationToken);

        return Result.Success();
    }
}
