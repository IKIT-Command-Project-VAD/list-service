namespace ShoppingList.List.UseCases.ListItems;

public record DeleteListItemCommand(Guid ListId, Guid OwnerId, Guid ItemId) : ICommand<Result>;

public sealed class DeleteListItemHandler(IRepository<ListItemEntity> itemRepo)
    : ICommandHandler<DeleteListItemCommand, Result>
{
    public async Task<Result> Handle(
        DeleteListItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var spec = new ListItemByIdSpec(request.ListId, request.ItemId);
        var item = await itemRepo.FirstOrDefaultAsync(spec, cancellationToken);
        if (item is null)
            return Result.NotFound();

        if (item.List?.OwnerId != request.OwnerId)
            return Result.NotFound();

        item.SoftDelete();
        await itemRepo.UpdateAsync(item, cancellationToken);
        return Result.Success();
    }
}

