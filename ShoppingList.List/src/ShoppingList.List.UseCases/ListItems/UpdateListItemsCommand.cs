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
            if (item is null || item.List is null)
            {
                logger.LogWarning(
                    "Item with ID {ItemId} not found in list {ListId}, skipping",
                    itemId,
                    request.ListId
                );
                continue;
            }

            bool isOwner = item.List.OwnerId == request.OwnerId;
            bool isMemberWithWrite = item.List.Members.Any(m => m.UserId == request.OwnerId && m.PermissionType == SharePermissionType.Write);

            if (!isOwner && !isMemberWithWrite)
            {
                logger.LogWarning(
                    "User {UserId} does not have permission to update item {ItemId} in list {ListId}, skipping",
                    request.OwnerId,
                    itemId,
                    request.ListId
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
